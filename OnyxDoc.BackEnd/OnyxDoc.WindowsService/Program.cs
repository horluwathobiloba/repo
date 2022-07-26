using System;
using System.ServiceProcess;

namespace OnyxRevamped.WindowsService
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new SendExpiryEmailNoticeService());
            ServiceBase.Run(new SendDocumentExpiryEmail());
            ServiceBase.Run(new SetDocumentToExpired());
            //ServiceBase.Run(new LoggingService());
        }
    }
}
