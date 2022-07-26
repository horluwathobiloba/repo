using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using Onyx.WindowsService.Utility;

namespace Onyx.WindowsService
{
    public partial class SetContractStatusToExpiredService : ServiceBase
    {
        private static Timer timer;
        private string _logFileLocation = @"C:\OnyxLogs\SetContractStatusToExpiredLog" + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".txt";

        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + logMessage + ":" + Environment.NewLine);
            string appName = ConfigurationManager.AppSettings["appName"];
            string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
            string setContractStatusToExpiredUrl = ConfigurationManager.AppSettings["setContractStatusToExoiredUrl"];

            APIClient aPIClient = new APIClient();
            var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
            var response = aPIClient.PostAPIUrl(setContractStatusToExpiredUrl, accessToken, new { name = appName }, false).Result;
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + response + ":" + Environment.NewLine);
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(10000); // 10 Seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Log("*******Stopping Onyx Set ContractStatus to Expired Service*******");
            timer.Stop();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SetContractStatusToExpiredService setContractStatusToExpiredService = new SetContractStatusToExpiredService();
            setContractStatusToExpiredService.Log("*******Starting Onyx Set ContractStatus to Expired Service*******");
        }

        protected override void OnPause()
        {
            Log("*******Pausing Onyx Set ContractStatus to Expired Service*******");
            base.OnPause();
        }

    }
}