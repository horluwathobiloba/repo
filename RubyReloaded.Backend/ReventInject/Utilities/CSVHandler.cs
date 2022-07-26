using System;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Linq;

namespace ReventInject.Utilities
{
    public class CSVHandler
	{

		//Returns datatable from CSV File2.    
		public System.Data.DataTable ReadCSV(string path)
		{
			StreamReader sr = new StreamReader(path);

			string fullFileStr = sr.ReadToEnd();
			sr.Close();
			sr.Dispose();
            string[] lines = fullFileStr.Split(ControlChars.Lf);
			DataTable recs = new DataTable();
			string[] sArr = lines[0].Split(',');

			foreach (string s in sArr) {
			}
			recs.Columns.Add(new DataColumn());
			 // ERROR: Not supported in C#: OnErrorStatement

			DataRow row = null;
			string finalLine = "";
			foreach (string line in lines) {
				row = recs.NewRow();
				finalLine = line.Replace(Convert.ToString(ControlChars.Cr), "");
				row.ItemArray = finalLine.Split(',');
				recs.Rows.Add(row);
				 // ERROR: Not supported in C#: OnErrorStatement

			}

			return recs;

		}

		public DataTable GetCSVData(string strFolderPath, string strFileName)
		{

			string strConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFolderPath + ";Extended Properties=Text;";
			OleDbConnection conn = new OleDbConnection(strConnString);

			try {
				conn.Open();

				OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + strFileName + "]", conn);
				OleDbDataAdapter da = new OleDbDataAdapter();
				da.SelectCommand = cmd;
				DataSet ds = new DataSet();
				da.Fill(ds);
				da.Dispose();

				return ds.Tables[0];
			} catch {
				return null;
			} finally {
				conn.Close();
			}

		}

		public static DataTable CSVToDataTable(string FileName, bool isRowOneHeader, bool isSavedInWebPath, string TextDelimeter = ",")
		{

			DataTable csvDataTable = new DataTable();

			try {
				string[] csvData = new string[]{};
				if (isSavedInWebPath) {
					//csvData = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath(FileName));
				} else {
					csvData = System.IO.File.ReadAllLines(FileName);
				}
				//if no data in file ‘manually’ throw an exception
				if (csvData.Length == 0) {
					throw new Exception("CSV File Appears to be Empty");
				}


				string[] headings = new string[]{};
				headings = csvData[0].Split(TextDelimeter.ToCharArray());
				if (headings != null) {
					if (headings.Count() > 0) {
						headings = headings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
					}
				}
				int index = 0;
				//will be zero or one depending on isRowOneHeader

				//if first record lists headers
				if (isRowOneHeader) {
					index = 1;
					//so we won’t take headings as data
					//for each heading
					for (int i = 0; i <= headings.Length - 1; i++) {
						//replace spaces with underscores for column names

						headings[i] = headings[i].Replace(" ", "_");
						//remove all unwanted xters
						headings[i] = headings[i].Replace("\"", null).Replace("'", null);
						//add a column for each heading
						csvDataTable.Columns.Add(headings[i]);
					}
				//if no headers just go for col1, col2 etc.
				} else {
					for (int i = 0; i <= headings.Length - 1; i++) {
						//remove all unwanted xters
						headings[i] = headings[i].Replace("\"", null).Replace("'", null);
						//create arbitary column names
						csvDataTable.Columns.Add("col" + (i + 1).ToString());
					}
				}

				//populate the DataTable
				for (int i = index; i <= csvData.Length - 1; i++) {
					//create new rows
					DataRow row = csvDataTable.NewRow();

					try {
						// do a check to ensure the row is not empty
						dynamic strRow = csvData[i].Replace("\"", null).Replace("'", null).Replace(",", null);
						if (string.IsNullOrEmpty(strRow)) {
							continue;
						}

						for (int j = 0; j <= headings.Length - 1; j++) {
							//fill them
							row[j] = csvData[i].Split(TextDelimeter.ToCharArray())[j];
							//remove all unwanted xters
							row[j] = row[j].ToString().Replace("\"", null).Replace("'", null);
						}

						//add rows to over DataTable
						csvDataTable.Rows.Add(row);
					} catch (Exception ex) {
						Console.WriteLine(ex.Message);
					}



				}

			} catch (Exception ex) {
			}
			//return the CSV DataTable
			return csvDataTable;

		}



	}
}

 