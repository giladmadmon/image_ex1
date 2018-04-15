using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// excute actions according to the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> the result of the execution </param>
        /// <returns>The String Will Return the New Path if result = true, and will return the error message</returns>
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}
