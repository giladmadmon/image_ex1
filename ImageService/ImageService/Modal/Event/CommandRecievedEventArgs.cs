using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class CommandRecievedEventArgs : EventArgs
    {
        #region Members
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRecievedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier of the command.</param>
        /// <param name="args">The arguments of the command.</param>
        /// <param name="path">The path of the handled directory.</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
