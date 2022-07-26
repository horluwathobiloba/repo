using ClosedXML.Excel;
using ReventInject.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ReventInject.Utilities
{
    public static class ExcelHelper //My_DataTable_Extensions
    {

        // Export DataTable into an excel file with field names in the header line
        // - Save excel file without ever making it visible if filepath is given
        // - Don't save excel file, just make it visible if no filepath is given
        public static void ExportToExcel(this DataTable dt, string FilePath)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                if (dt == null || dt.Columns.Count == 0)
                {
                    throw new Exception("ExportToExcel: Null or empty input table!");
                }

                if (string.IsNullOrEmpty(FilePath))
                {
                    throw new Exception("ExportToExcel: file path must be specified");
                }

                string dir = FilePath.Substring(0, FilePath.LastIndexOf("\\"));

                if (!(Directory.Exists(dir)))
                {
                    Directory.CreateDirectory(dir);
                }

                // load excel, and create a new workbook

                excelApp.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet workSheet = (Microsoft.Office.Interop.Excel._Worksheet)excelApp.ActiveSheet;

                // column headings
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    workSheet.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;
                }

                // rows
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        workSheet.Cells[(i + 2), (j + 1)] = dt.Rows[i][j];
                    }
                }

                // check fielpath
                if (FilePath != null && FilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(FilePath);
                        excelApp.Quit();
                        Console.WriteLine("Excel file saved! Path -> " + FilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath."
                            + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: " + ex.Message);
            }
            finally
            {
                excelApp.Visible = true;
                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
                KillSpecificExcelFileProcess(Path.GetFileName(FilePath));

            }

        }

        public static void ExportToExcel<T>(this IEnumerable<T> list, string FilePath)
        {
            DataTable Tbl = DALExtensions.CollectionHelper.ConvertTo<T>(list.ToList());

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                {
                    throw new Exception("ExportToExcel: Null or empty input table!");
                }

                if (string.IsNullOrEmpty(FilePath))
                {
                    throw new Exception("ExportToExcel: file path must be specified");
                }

                string dir = FilePath.Substring(0, FilePath.LastIndexOf("\\"));

                if (!(Directory.Exists(dir)))
                {
                    Directory.CreateDirectory(dir);
                }

                //if (!(File.Exists(FilePath)))
                //{
                //    File.Create(FilePath);
                //}


                // load excel, and create a new workbook

                excelApp.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet workSheet = (Microsoft.Office.Interop.Excel._Worksheet)excelApp.ActiveSheet;

                // column headings
                for (int i = 0; i < Tbl.Columns.Count - 1; i++)
                {
                    workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;
                }

                // rows
                for (int i = 0; i < Tbl.Rows.Count - 1; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
                    }
                }

                // check fielpath
                if (FilePath != null && FilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(FilePath);
                        excelApp.Quit();
                        Console.WriteLine("Excel file saved! Path -> " + FilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: " + ex.Message);
            }
            finally
            {
                excelApp.Visible = true;
                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
                KillSpecificExcelFileProcess(Path.GetFileName(FilePath));

            }

        }

        private static void KillSpecificExcelFileProcess(string excelFileName)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;

            foreach (var process in processes)
            {
                if (process.MainWindowTitle == "Microsoft Excel - " + excelFileName)
                    process.Kill();
            }
        }

        public static byte[] ExportToExcelByte<T>(this IEnumerable<T> data, string worksheetTitle)
        {
            var wb = new XLWorkbook(); //create workbook
            var ws = wb.Worksheets.Add(worksheetTitle); //add worksheet to workbook

            bool startFirstRow = false;


            //start inserting the data

            if (data != null)
            {
                if (data.Count() > 0)
                {
                    if (startFirstRow)
                    {
                        //insert data to from second row on
                        ws.Cell(1, 1).InsertData(data);
                    }
                    else
                    {
                        //insert data to from second row on
                        ws.Cell(2, 1).InsertData(data);
                    }
                }
            }


            //save file to memory stream and return it as byte array
            using (var ms = new MemoryStream())
            {
                wb.SaveAs(ms);

                return ms.ToArray();
            }
        }

        public static void ExportToExcel<T>(this IEnumerable<T> list, string SheetName, string FilePath)
        {

            try
            {
                DataTable dt = DALExtensions.CollectionHelper.ConvertTo<T>(list.ToList());
                
                //Exporting to Excel
                if (dt == null || dt.Columns.Count == 0)
                {
                    throw new Exception("ExportToExcel: Null or empty input table!");
                }

                if (string.IsNullOrEmpty(FilePath))
                {
                    throw new Exception("ExportToExcel: file path must be specified");
                }

                string dir = FilePath.Substring(0, FilePath.LastIndexOf("\\"));

                if (!(Directory.Exists(dir)))
                {
                    Directory.CreateDirectory(dir);
                }


                using (XLWorkbook wb = new XLWorkbook())
                {
                    try
                    {
                        wb.Worksheets.Add(dt);
                        wb.SaveAs(FilePath);
                        Console.WriteLine("Excel file saved! Path -> " + FilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: " + ex.Message);
            }
            finally
            {
                KillSpecificExcelFileProcess(Path.GetFileName(FilePath));

            }

        }

        public static MemoryStream GetExcelStream<T>(this IEnumerable<T> list)
        {
            try
            {
                DataTable dt = DALExtensions.CollectionHelper.ConvertTo<T>(list.ToList());
                //Exporting to Excel
                if (dt == null || dt.Columns.Count == 0)
                {
                    throw new Exception("ExportToExcel: Null or empty input table!");
                }

                MemoryStream fs = new MemoryStream();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    try
                    {
                        wb.Worksheets.Add(dt);
                        wb.SaveAs(fs);
                        fs.Position = 0;
                        Console.WriteLine("Excel file saved as file stream");
                        return fs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath." + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: " + ex.Message);
            }
        }

    }

}
