using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
        }

        public void createHandler(string directory)
        {
            IDirectoryHandler dirHandler = new DirectoyHandler(m_controller, m_logging);

            CommandRecieved += dirHandler.OnCommandRecieved;
            dirHandler.DirectoryClose += CloseHanlder;

            dirHandler.StartHandleDirectory(directory.Trim());
        }

        public void sendCommand(CommandEnum commandId, string[] args, string path)
        {
            CommandRecievedEventArgs cmdEventArgs = new CommandRecievedEventArgs((int)commandId, args, path);
            CommandRecieved(this, cmdEventArgs);
            // LOG???
            m_logging.Log(DateTime.Now + path + " - " + commandId, Logging.Modal.MessageTypeEnum.INFO);
        }
        //////// – closes handlers
        private void CloseHanlder(object sender, DirectoryCloseEventArgs eventArgs)
        {
            IDirectoryHandler dirHandler = (IDirectoryHandler)sender;
            CommandRecieved -= dirHandler.OnCommandRecieved;
            dirHandler.DirectoryClose -= CloseHanlder;
        }

        public void CloseServer()
        {
            sendCommand(CommandEnum.CloseCommand, new string[] { }, "");
        }
        //– handler will call this function to tell server it closed
    }
}
