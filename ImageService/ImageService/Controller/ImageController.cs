using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;              // The Dictionary Object

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="modal">The modal.</param>
        public ImageController(IImageServiceModal modal)
        {
            // Storing the Modal Of The System
            m_modal = modal;

            // Opening a Dictionary of commands
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.NewFileCommand, new NewFileCommand(m_modal) }
            };
        }

        /// <summary>
        /// Executing the Command Requested
        /// </summary>
        /// <param name="commandID"> the command id of the command to be executed </param>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the command </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}
