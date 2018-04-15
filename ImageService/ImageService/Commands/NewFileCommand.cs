using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// creating new file command
        /// </summary>
        /// <param name="modal"> the modal which is responsible to do file actions </param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// excute actions according to the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the execution </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        public string Execute(string[] args, out bool result)
        {
            string path = args[0];
            return m_modal.AddFile(path, out result);
        }
    }
}
