using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace OnyxDocSubscription.WindowsService
{
   public class LoggingService : ServiceBase
    {
        private const string _logFileLocation = @"C:\temp\servicelog.txt";

        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }

        protected override void OnStart(string[] args)
        {
            Log("Starting Onyx Windows Service");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Log("Stopping Onyx Windows Service");
            base.OnStop();
        }

        protected override void OnPause()
        {
            Log("Pausing Onyx Windows Service");
            base.OnPause();
        }
    }
}
