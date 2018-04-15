using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        #region Members
        public string DirectoryPath { get; set; }

        public string Message { get; set; }             // The Message That goes to the logger
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCloseEventArgs"/> class.
        /// </summary>
        /// <param name="dirPath">The path of the closed dir.</param>
        /// <param name="message">The message to print in the log.</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}
