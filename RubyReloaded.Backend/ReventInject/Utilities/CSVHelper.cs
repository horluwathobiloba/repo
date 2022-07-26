using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using ReventInject.DataAccess;


namespace ReventInject.Utilities
{
    public static class CSVHelper
    {

        public static void ExportToCSV(this DataTable data, string strPath, bool HasColumnHeader = true, char TextDelimiter = ',', char TextQualifier = '"')
        {
            bool resp = new CSVWriter().Save(data, strPath, HasColumnHeader, TextDelimiter, TextQualifier);

            KillSpecificExcelFileProcess(Path.GetFileName(strPath));
        }

        public static void ExportToCSV<T>(this IEnumerable<T> list, string strPath, bool HasColumnHeader = true, char TextDelimiter = ',', char TextQualifier = '"')
        {
            DataTable data = DALExtensions.CollectionHelper.ConvertTo<T>(list.ToList());

            bool resp = new CSVWriter().Save(data, strPath, HasColumnHeader, TextDelimiter, TextQualifier); 
            KillSpecificExcelFileProcess(Path.GetFileName(strPath));
        }          

        private static void KillSpecificExcelFileProcess(string csvFileName)
        {
            try
            {
                var processes = from p in Process.GetProcessesByName("EXCEL")
                                select p;

                foreach (var process in processes)
                {
                    if (process.MainWindowTitle == "Microsoft Excel - " + csvFileName)
                        process.Kill();
                }

                //OR run this just in case the machine is set to open the files using notepad.
                var notepadProcesses = from p in Process.GetProcessesByName("NOTEPAD")
                                       select p;

                foreach (var process in notepadProcesses)
                {
                    if (process.MainWindowTitle == "Microsoft Excel - " + csvFileName)
                        process.Kill();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}

