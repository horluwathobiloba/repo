using OnyxRevamped.WindowsService.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace OnyxRevamped.WindowsService
{
    public class SetDocumentToExpired : ServiceBase
    {
        private const string _logFileLocation = @"C:\temp\servicelog.txt";
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));

            string appName = ConfigurationManager.AppSettings["appName"];
            string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
            string expiryEmailUrl = ConfigurationManager.AppSettings["getdocumentsstatusbysubscriberidUrl"];

            APIClient aPIClient = new APIClient();
            var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
            var response = aPIClient.PostAPIUrl(expiryEmailUrl, accessToken, new { name = appName }, false).Result;
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + response + ":" + Environment.NewLine);
        }

        protected override void OnStart(string[] args)
        {
            Log("Starting OnyxRevamped Windows Service");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Log("Stopping OnyxRevamped Windows Service");
            base.OnStop();
        }

        protected override void OnPause()
        {
            Log("Pausing OnyxRevamped Windows Service");
            base.OnPause();
        }

    }
}

