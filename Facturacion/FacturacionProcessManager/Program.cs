using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FacturacionProcessManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (!DEBUG)

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FacturacionProcessManager()
            };
            ServiceBase.Run(ServicesToRun);

#else
            FacturacionProcessManager mainService = new FacturacionProcessManager();
            String[] args = new String[] { String.Empty };
            mainService.OnStart(args);
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
}
