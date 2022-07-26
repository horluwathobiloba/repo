using System;
using System.ServiceProcess;

namespace Onyx.WindowsService
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
          {
               new SendApprovalEmailService(),
               new SendExpiryEmailNoticeService(),
               new SetContractStatusToExpiredService()

          };
            ServiceBase.Run(servicesToRun);
            //ServiceBase.Run(new SendApprovalEmailService());
            //ServiceBase.Run(new SendExpiryEmailNoticeService());
            //ServiceBase.Run(new LoggingService());
        }
    }
}
