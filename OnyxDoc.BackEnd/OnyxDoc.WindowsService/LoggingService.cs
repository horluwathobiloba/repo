using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace OnyxRevamped.WindowsService
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
