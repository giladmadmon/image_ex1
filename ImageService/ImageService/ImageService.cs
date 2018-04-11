using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using System.Security.Permissions;

public enum ServiceState
{
    SERVICE_STOPPED = 0x00000001,
    SERVICE_START_PENDING = 0x00000002,
    SERVICE_STOP_PENDING = 0x00000003,
    SERVICE_RUNNING = 0x00000004,
    SERVICE_CONTINUE_PENDING = 0x00000005,
    SERVICE_PAUSE_PENDING = 0x00000006,
    SERVICE_PAUSED = 0x00000007,
}

[StructLayout(LayoutKind.Sequential)]
public struct ServiceStatus
{
    public int dwServiceType;
    public ServiceState dwCurrentState;
    public int dwControlsAccepted;
    public int dwWin32ExitCode;
    public int dwServiceSpecificExitCode;
    public int dwCheckPoint;
    public int dwWaitHint;
};

namespace ImageService
{
    public partial class x : ServiceBase
    {
        private ImageServer m_imageServer;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public x()
        {
            InitializeComponent();

            string sourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];

            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, logName);
            }

            this.EventLogger.Source = sourceName;
        }

        protected override void OnStart(string[] args)
        {
            string outptDir = ConfigurationManager.AppSettings["OutputDir"];
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThunmbnailSize"]);
            string[] handlers = ConfigurationManager.AppSettings["Handler"].Split(';');

            IImageServiceModal modal = new ImageServiceModal(outptDir, thumbnailSize);
            IImageController controller = new ImageController(modal);
            ILoggingService logging = new LoggingService();
            this.m_imageServer = new ImageServer(controller, logging);

            logging.MessageRecieved += (sender, msgReceived) =>
            {
                this.EventLogger.WriteEntry(msgReceived.Message, (EventLogEntryType)msgReceived.Status);
            };

            foreach (string handler in handlers)
            {
                this.m_imageServer.createHandler(handler);
            }

            this.EventLogger.WriteEntry("Service Started");
        }

        protected override void OnStop()
        {
            m_imageServer.CloseServer();
            this.EventLogger.WriteEntry("Service Stopped");
        }

        private void EventLogger_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
