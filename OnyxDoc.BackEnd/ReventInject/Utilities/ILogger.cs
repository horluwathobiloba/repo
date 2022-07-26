using Microsoft.Extensions.Configuration;
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

    public interface ILogger
    {

        void WriteToEventLog(string message, EventLogEntryType EventType = EventLogEntryType.Information, string LogName = "Application");

        void WriteErrorToEventLog(Exception ex, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");
        void WriteToEventLog(Exception ex, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");
        void WriteErrorToEventLog(string message, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");

        void LogError(Exception ex, string ObjName = "ErrorLog");

        void LogInfo(string err, string ObjName = "ErrorLog");

        void SaveFile(string content, string FileName, string FolderName = "ReventInject");

        void WriteToEventLogAsync(string message, EventLogEntryType EventType = EventLogEntryType.Information, string LogName = "Application");

        public void WriteErrorToEventLogAsync(Exception ex, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");

        void WriteToEventLogAsnyc(Exception ex, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");

        void WriteErrorToEventLogAsnyc(string message, EventLogEntryType EventType = EventLogEntryType.Error, string LogName = "ReventInject");

        void LogErrorAsync(Exception ex, string ObjName = "ErrorLog");

        void LogInfoAsnyc(string err, string ObjName = "ErrorLog");

        void SaveFileAsnyc(string content, string FileName, string FolderName = "ReventInject");

    }
}



