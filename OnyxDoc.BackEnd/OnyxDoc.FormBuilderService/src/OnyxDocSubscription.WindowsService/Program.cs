using System;
using System.ServiceProcess;

namespace OnyxDocSubscription.WindowsService
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
          {
               new FormBuilderService(),

          };
            ServiceBase.Run(servicesToRun);
        }
    }
}
