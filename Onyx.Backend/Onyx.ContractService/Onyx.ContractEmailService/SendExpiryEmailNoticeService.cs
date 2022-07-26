using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractEmailService
{
    public class SendExpiryEmailNoticeService : ServiceBase
    {
        System.Timers.Timer timer;
        private string startMsg = "Onyx Expiry Email Notice Service started";
        private string stopMsg = "Onyx Expiry Email Notice Service stopped";
        private string subject = "Onyx Expiry Email Notice Service - Service Monitor";
        private static DateTime next_run = DateTime.Now;

        protected async override void OnStart(string[] args)
        {
            try
            {
                timer = new System.Timers.Timer(60000);
                int timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();
                string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                //get api key first
                string appName = "AnellohServices";
                int appType = 4;
                var apiKey = await aPIClient.PostAPIUrl(apiUrl, "", new { name = appName, applicationType = appType }, false);
                //then call fulfill matched orders API
                await aPIClient.PostAPIUrl(apiUrl, apiKey);

            }
            catch (Exception ex)
            {
                Logger.WriteToEventLog(ex?.Message ?? ex?.InnerException?.Message, "Anelloh", EventLogEntryType.Error, "AnellohFulfillMatchService");
            }
        }

        protected override void OnStop()
        {
            try
            {
                timer.Stop();
                timer.Dispose();

            }
            catch (Exception ex)
            {
                Logger.WriteToEventLog(ex?.Message ?? ex?.InnerException?.Message, "Anelloh", EventLogEntryType.Error, "AnellohFulfillMatchService");
            }
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            try
            {
                if (DateTime.Now > next_run)
                {
                    // eSender = new EmailService();
                    string msg = "Anelloh Payment Service process started @ " + string.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now);

                    //send message and log to event viewer
                    // eSender.SendEmailAlert(dList, null, null, msg, "AnellohFulfillMatchService", subject);
                    Logger.WriteToEventLog(msg, "AnellohFulfillMatchService", EventLogEntryType.Information, "AnellohFulfillMatchService");

                    //run process                    


                    //send message and log to event viewer
                    msg = "All processes completed @ " + string.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now);
                    Logger.WriteToEventLog(msg, "NFIUServices", EventLogEntryType.Information, "AnellohFulfillMatchService");
                }
                else
                {
                    Logger.WriteToEventLog("Anelloh Payment Service is alive and running...", "Anelloh", EventLogEntryType.Information, "AnellohFulfillMatchService");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteToEventLog(ex?.Message ?? ex?.InnerException?.Message, "Anelloh", EventLogEntryType.Error, "AnellohFulfillMatchService");
            }

            timer.Start();
            Thread.Sleep(10000); // sleep for 10secs
        }
    }
}

