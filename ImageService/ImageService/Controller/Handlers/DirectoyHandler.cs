using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {

        #region Members
        private static readonly string[] filters = { ".jpg", ".png", ".gif", ".bmp" }; // the extensions to be monitored
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        #region Properties
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoyHandler"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="logging">The logger which is responsible for the log.</param>
        public DirectoyHandler(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_path = null;
        }

        // Define the event handlers.
        /// <summary>
        /// Called when a file is created.
        /// </summary>
        /// <param name="source">The object which called the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            if (filters.Contains(Path.GetExtension(e.FullPath)))
            {
                // Specify what is done when a file is changed, created, or deleted.
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(
                    (int)CommandEnum.NewFileCommand, new string[] { e.FullPath }, m_path);

                OnCommandRecieved(this, eventArgs);
            }
        }

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath">the directory to handle</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;

            // Create a new FileSystemWatcher and set its properties.
            m_dirWatcher = new FileSystemWatcher();
            m_dirWatcher.Path = m_path;

            // Watch for changes in CreatinTime
            m_dirWatcher.NotifyFilter = NotifyFilters.FileName; 
               
            // Add event handlers.
            m_dirWatcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
            m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// When a command comes- checks what kind it is: if closing- its closes the service,
        /// otherwise its calls to ExecuteCommand and writes to the log sucsess\failure
        /// </summary>
        /// <param name="sender">the object called to the event</param>
        /// <param name="e">the event args required for this event</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string message;
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                DirectoryCloseEventArgs dirCloseArgs = new DirectoryCloseEventArgs(m_path, "CLOSE");
                DirectoryClose(this, dirCloseArgs);
                return;
            }
            else
            {

                if (m_path == null || !e.RequestDirPath.Contains(m_path))
                    return;

                message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            }

            // write to the log
            if (result)
            {
                m_logging.Log(message, MessageTypeEnum.INFO);
            }
            else
            {
                m_logging.Log(message, MessageTypeEnum.FAIL);
            }

        }
    }
}
