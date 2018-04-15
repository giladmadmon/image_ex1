using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath"> the directory to handle </param>
        void StartHandleDirectory(string dirPath);

        /// <summary>
        /// The Event that will be activated upon new Command.
        /// </summary>
        /// <param name="sender"> the object called to the event </param>
        /// <param name="e"> the event args required for this event </param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // 
    }
}
