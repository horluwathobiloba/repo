using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;


namespace ReventInject.Utilities
{
    public class CSVWriter
    {

        private char _TextDelimiter;
        private char _TextQualifiers;
        private bool _HasColumnHeaders;

        public CSVWriter()
        {
            TextDelimiter = ',';
            TextQualifiers = '"';
            HasColumnHeaders = true;
        }

        public CSVWriter(char _TextDelimeter)
        {
            TextDelimiter = _TextDelimeter;
            TextQualifiers = '"';
            HasColumnHeaders = true;
        }

        public CSVWriter(char _TextDelimeter, char _TextQualifiers)
        {
            TextDelimiter = _TextDelimeter;
            TextQualifiers = _TextQualifiers; 
            HasColumnHeaders = true;
        }

        public CSVWriter(char _TextDelimeter, char _TextQualifiers, bool _HasColumnHeaders)
        {
            TextDelimiter = _TextDelimeter;
            TextQualifiers = _TextQualifiers;
            HasColumnHeaders = _HasColumnHeaders;
        }

        public char TextDelimiter
        {

            get { return _TextDelimiter; }
            set { _TextDelimiter = value; }
        }

        public char TextQualifiers
        {
            get { return _TextQualifiers; }
            set { _TextQualifiers = value; }
        }

        public bool HasColumnHeaders
        {
            get { return _HasColumnHeaders; }
            set { _HasColumnHeaders = value; }
        }

        private bool Save(DataTable data, string strPath)
        {
            try
            {
                using (StreamWriter _CsvWriter = new StreamWriter(strPath))
                {
                    _CsvWriter.Write(DataTableToCSV(data, strPath));
                    _CsvWriter.Flush();
                    _CsvWriter.Dispose();
                    _CsvWriter.Close();
                }

                //OpenAccess the file
                //System.Diagnostics.Process.Start(strPath)
                Console.WriteLine("CSV saved!");
                return true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
                return false;
            }
        }


        public bool Save(DataTable data, string strPath, bool _HasColumnHeader = false, char _TextDelimiter = ',', char _TextQualifier = '"')
        {
            HasColumnHeaders = _HasColumnHeader;
            TextDelimiter = _TextDelimiter;
            TextQualifiers = _TextQualifier;

            return Save(data, strPath);

        }

        private string DataTableToCSV(DataTable InputTable, string savePath)
        {
            string functionReturnValue = null;
            StringBuilder CsvBuilder = new StringBuilder();
            try
            {
                if (HasColumnHeaders)
                {
                    CreateHeader(InputTable, CsvBuilder);
                }
                CreateRows(InputTable, CsvBuilder);
                return CsvBuilder.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
                functionReturnValue = null;
            }
            return functionReturnValue;

        }

        public string DataTableToCSV(DataTable InputTable)
        {
            StringBuilder CsvBuilder = new StringBuilder();
            if (HasColumnHeaders)
            {
                CreateHeader(InputTable, CsvBuilder);
            }
            CreateRows(InputTable, CsvBuilder);
            return CsvBuilder.ToString();
        }


        private void CreateRows(DataTable InputTable, StringBuilder CsvBuilder)
        {
            foreach (DataRow ExportRow in InputTable.Rows)
            {
                int count = 1;
                foreach (DataColumn ExportColumn in InputTable.Columns)
                {
                    string ColumnText = ExportRow[ExportColumn.ColumnName].ToString();
                    ColumnText = ColumnText.Replace(TextQualifiers.ToString(), TextQualifiers.ToString() + TextQualifiers.ToString());
                    CsvBuilder.Append(TextQualifiers + ColumnText + TextQualifiers);
                    if (count < InputTable.Columns.Count)
                    {
                        CsvBuilder.Append(TextDelimiter);
                    }
                    count = count + 1;
                }
                CsvBuilder.AppendLine();
            }
        }

        private void CreateHeader(DataTable InputTable, StringBuilder CsvBuilder)
        {
            int count = 1;
            foreach (DataColumn ExportColumn in InputTable.Columns)
            {
                string ColumnText = ExportColumn.ColumnName.ToString();
                ColumnText = ColumnText.Replace(TextQualifiers.ToString(), TextQualifiers.ToString() + TextQualifiers.ToString());
                CsvBuilder.Append(TextQualifiers + ExportColumn.ColumnName + TextQualifiers);
                if (count < InputTable.Columns.Count)
                {
                    CsvBuilder.Append(TextDelimiter);
                }
                count = count + 1;
            }
            CsvBuilder.AppendLine();
        }


    }
}

