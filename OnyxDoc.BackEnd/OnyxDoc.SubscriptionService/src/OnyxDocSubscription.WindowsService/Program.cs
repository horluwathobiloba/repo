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
               new SubscriptionService(),

          };
            ServiceBase.Run(servicesToRun);
        }
    }
}
