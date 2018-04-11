using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {






            //starts the program with the given parameters at the AppConfiguration file
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                {
                new x()
                };
            ServiceBase.Run(ServicesToRun);

        }

    }
}
