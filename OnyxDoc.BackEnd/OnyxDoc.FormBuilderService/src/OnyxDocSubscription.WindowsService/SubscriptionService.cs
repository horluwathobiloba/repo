
using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Application.Subscriptions.Commands;
using OnyxDoc.FormBuilderService.Infrastructure.Services;
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

namespace OnyxDocSubscription.WindowsService
{
    public partial class FormBuilderService : ServiceBase
    {
        private static Timer timer;
        private string _logFileLocation = @"C:\OnyxDocLogs\FormBuilderServiceLog" + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".txt";
        private readonly IConfiguration _configuration;
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + logMessage + ":" + Environment.NewLine);
            string appName = ConfigurationManager.AppSettings["appName"];
            string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];

            BearerTokenService aPIClient = new BearerTokenService(_configuration);
            var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
            //Todo : get admin user and finalize
            DeactivateExpiredSubscriptionsCommand command = new DeactivateExpiredSubscriptionsCommand { AccessToken = accessToken, SuperAdminSubscriberId = 1, UserId = "" };
            File.AppendAllText(_logFileLocation, DateTime.Now.ToString()  + command + ":" + Environment.NewLine);
        }


        protected override void OnStart(string[] args)
        {
            timer = new Timer(10000); // 10 Seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Log("*******Stopping OnyxDoc Subscription  Windows Service*******");
            timer.Stop();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            FormBuilderService formBuilderService = new FormBuilderService();
            formBuilderService.Log("*******Starting OnyxDoc Subscription  Windows Service*******");
        }

        protected override void OnPause()
        {
            Log("*******Pausing OnyxDoc Subscription  Windows Service*******");
            base.OnPause();
        }

    }
}
