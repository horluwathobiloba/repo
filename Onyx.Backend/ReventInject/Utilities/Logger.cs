using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.Utilities
{

    public class Logger
    {
        private static string path = SysConfig.AppFolder;
        public static void WriteToEventLog(string message, string AppName = "ReventInject", EventLogEntryType EventType = EventLogEntryType.Information, string LogName = "Application")
        {

            EventLog objEventLog = new EventLog();

            try
            {
                //Register the App as an Event Source

                if (!EventLog.SourceExists(AppName))
                {
                    EventLog.CreateEventSource(AppName, LogName);
                }

                objEventLog.Source = AppName;

                //WriteEntry is overloaded; this is one
                //of 10 ways to call it
                objEventLog.WriteEntry(message, EventType);
            }
            catch (Exception Ex)
            {
            }

        }

        public static void WriteErrorToEventLog(Exception ex, string AppName = "ReventInject",
            EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            try
            {
                message += string.Format("TargetSite: {0}", ex.TargetSite == null ? null : ex.TargetSite.ToString());
            }
            catch
            {
            }
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            EventLog objEventLog = new EventLog();

            try
            {
                //Register the App as an Event Source
                if (!EventLog.SourceExists(LogName))
                {
                    EventLog.CreateEventSource(AppName, LogName);
                }

                objEventLog.Source = AppName;
                objEventLog.WriteEntry(message, EventType);
            }
            catch (Exception Exx)
            {

            }

        }

        public static void WriteToEventLog(Exception ex, string AppName = "ReventInject",
          EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            try
            {
                message += string.Format("TargetSite: {0}", ex.TargetSite == null ? null : ex.TargetSite.ToString());
            }
            catch
            {
            }
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            EventLog objEventLog = new EventLog();

            try
            {
                //Register the App as an Event Source
                if (!EventLog.SourceExists(LogName))
                {
                    EventLog.CreateEventSource(AppName, LogName);
                }

                objEventLog.Source = AppName;
                objEventLog.WriteEntry(message, EventType);
            }
            catch (Exception Exx)
            {

            }

        }


        public static void WriteErrorToEventLog(string message, string AppName = "ReventInject",
          EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            EventLog objEventLog = new EventLog();
            try
            {
                //Register the App as an Event Source
                if (!EventLog.SourceExists(LogName))
                {
                    EventLog.CreateEventSource(AppName, LogName);
                }

                objEventLog.Source = AppName;
                objEventLog.WriteEntry(message, EventType);
            }
            catch (Exception Exx)
            {

            }

        }


        public static void LogError(Exception ex, string ObjName = "ErrorLog", string AppName = "ReventInject")
        {
            var folder = string.Format("{0}{1}\\", path, ObjName);

            if (!Directory.Exists(string.Format(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            string fullPath = string.Format("{0}{1:ddMMyyyy-hh}.txt", folder, DateTime.Now);

            string message = string.Format("** Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += "\n-----------------------------------------------------------\n";
            message += string.Format("AppName: {0}", AppName);
            message += "\n-----------------------------------------------------------\n";
            message += "Message:\n";
            message += string.Format("{0}", ex.ExceptionMessage());
            message += string.Format("\nStackTrace: {0}", ex.StackTrace);
            message += string.Format("\nSource: {0}", ex.Source);
            message += string.Format("\nTargetSite: {0}", ex.TargetSite.ToString());
            message += "\n**";

            using (StreamWriter writer = new StreamWriter(fullPath, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void Log(string err, string ObjName = "ErrorLog", string AppName = "ReventInject")
        {

            var folder = string.Format("{0}{1}\\", path, ObjName);

            if (!Directory.Exists(string.Format(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            string fullPath = string.Format("{0}{1:ddMMyyyy-hh}.txt", folder, DateTime.Now);


            StringBuilder message = new StringBuilder(string.Format(" **Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            message.AppendLine("****************************");
            message.AppendLine(string.Format("AppName: {0}", AppName));
            message.AppendLine("****************************");
            message.AppendLine("Message:");
            message.AppendLine(string.Format("{0}", err));
            message.AppendLine("**");
            using (StreamWriter writer = new StreamWriter(fullPath, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void SaveFile(string content, string FileName, string FolderName = "QSB")
        {

            var folder = string.Format("{0}{1}\\{2}\\", path, FolderName, string.Format("{0:ddMMyyyy}.txt", DateTime.Now));

            if (!Directory.Exists(string.Format(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            string fullPath = string.Format("{0}{1}.txt", folder, FileName);


            StringBuilder message = new StringBuilder();//string.Format(" **Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            //message.AppendLine("");
            message.AppendLine(string.Format("{0}", content));
            using (StreamWriter writer = new StreamWriter(fullPath, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void WriteToEventLogAsync(string message, string AppName = "ReventInject", EventLogEntryType EventType = EventLogEntryType.Information, string LogName = "Application")
        {
            Task.Factory.StartNew(() => WriteToEventLog(message, AppName, EventType, LogName));
        }

        public static void WriteErrorToEventLoAsynch(Exception ex, string AppName = "ReventInject", EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            Task.Factory.StartNew(() => WriteErrorToEventLog(ex, AppName, EventType, LogName));
        }

        public static void WriteToEventLogAsnyc(Exception ex, string AppName = "ReventInject",
         EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            Task.Factory.StartNew(() => WriteToEventLog(ex, AppName, EventType, LogName));
        }

        public static void WriteErrorToEventLogAsnyc(string message, string AppName = "ReventInject",
          EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject")
        {
            Task.Factory.StartNew(() => WriteErrorToEventLog(message, AppName, EventType, LogName));
        }

        public static void LogErrorAsync(Exception ex, string ObjName = "ErrorLog", string AppName = "ReventInject")
        {
            Task.Factory.StartNew(() => LogError(ex, ObjName, AppName));
        }

        public static void LogAsnyc(string err, string ObjName = "ErrorLog", string AppName = "ReventInject")
        {
            Task.Factory.StartNew(() => Log(err, ObjName, AppName));
        }

        public static void SaveFileAsnyc(string content, string FileName, string FolderName = "ReventInject")
        {
            Task.Factory.StartNew(() => SaveFile(content, FileName, FolderName));
        }

    }
}



