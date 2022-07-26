using Microsoft.Extensions.Logging;
using Onyx.WindowsService.Utility;
using ReventInject.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Onyx.WindowsService
{
    public partial class SendExpiryEmailNoticeService : ServiceBase
    {
        private static Timer timer;
        private string _logFileLocation = @"C:\OnyxLogs\ExpiryEmailServiceLog" + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".txt";
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + logMessage + ":" + Environment.NewLine);
            string appName = ConfigurationManager.AppSettings["appName"];
            string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
            string expiryEmailUrl = ConfigurationManager.AppSettings["contractExpiryUrl"];

            APIClient aPIClient = new APIClient();
            var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
            var response = aPIClient.PostAPIUrl(expiryEmailUrl, accessToken, new { name = appName }, false).Result;
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString()  + response + ":" + Environment.NewLine);
        }

     
        protected override void OnStart(string[] args)
        {
            timer = new Timer(10000); // 10 Seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Log("*******Stopping Onyx Expiry Email Windows Service*******");
            timer.Stop();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SendExpiryEmailNoticeService expiryEmailService = new SendExpiryEmailNoticeService();
            expiryEmailService.Log("*******Starting Onyx Expiry Email Windows Service*******");
        }

        protected override void OnPause()
        {
            Log("*******Pausing Onyx Expiry Email Windows Service*******");
            base.OnPause();
        }

    }
}

