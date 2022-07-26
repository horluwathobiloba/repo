using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ReventInject.Utilities
{
    public class CSVReader
	{
		//
		private Stream objStream;
		private StreamReader objReader;
		//add name space System.IO.Stream
		public CSVReader(Stream filestream) : this(filestream, null)
		{
		}

		public CSVReader(Stream filestream, Encoding enc)
		{
			this.objStream = filestream;
			//check the Pass Stream whether it is readable or not
			if ((!filestream.CanRead)) {
				return;
			}
			if (((enc != null))) {
				objReader = new StreamReader(filestream, enc);
			} else {
				objReader = new StreamReader(filestream);
			}
		}

		//parse the Line
		public string[] GetCSVLine()
		{
			string data = objReader.ReadLine();
			if (data == null) {
				return null;
			}
			if (data.Length == 0) {
				return new string[]{};
			}
			//System.Collection.Generic
			ArrayList result = new ArrayList();
			//parsing CSV Data
			ParseCSVData(result, data);
            return (string[])(result.ToArray(typeof(string)));
		}

		private void ParseCSVData(ArrayList result, string data)
		{
			int position = -1;
			while (position < data.Length) {
				result.Add(ParseCSVField(ref data, ref position));
			}
		}

		private string ParseCSVField(ref string data, ref int StartSeperatorPos)
		{
			if (StartSeperatorPos == data.Length - 1) {
				StartSeperatorPos += 1;
				return "";
			}
			int fromPos = StartSeperatorPos + 1;
			if (data[fromPos] == '"') {
				int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
				int lines = 1;
				while (nextSingleQuote == -1) {
					data = data + "\n" + objReader.ReadLine();
					nextSingleQuote = GetSingleQuote(data, fromPos + 1);
					lines += 1;
					if (lines > 20) {
						throw new Exception("lines overflow: " + data);
					}
				}
				StartSeperatorPos = nextSingleQuote + 1;
				string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
				tempString = tempString.Replace("'", "''");
				return tempString.Replace("\"\"", "\"");
			}
			int nextComma = data.IndexOf(',', fromPos);
			if (nextComma == -1) {
				StartSeperatorPos = data.Length;
				return data.Substring(fromPos);
			} else {
				StartSeperatorPos = nextComma;
				return data.Substring(fromPos, nextComma - fromPos);
			}
		}

		private int GetSingleQuote(string data, int SFrom)
		{
			int i = SFrom - 1;
			//TODO: INSTANT VB TODO TASK: Assignments within expressions are not supported in VB.NET
			//ORIGINAL LINE: while (++i < data.Length)
			while (+i < data.Length) {
				if (data[i] == '"') {
					if (i < data.Length - 1 && data[i + 1] == '"') {
						i += 1;
						continue;
					} else {
						return i;
					}
				}
			}
			return -1;
		}

		public string ReadFile(string filepath, string Delimiter)
		{
			//Dim filetoread As String
			//filetoread = server.mappath("readtest.txt")
			string result = "";
			StreamReader filestream = null;
			filestream = File.OpenText(filepath);
			string readcontents = null;
			readcontents = filestream.ReadToEnd();
			string textdelimiter = Delimiter;
            var splitout = readcontents.Split(textdelimiter.ToCharArray());
			result = readcontents + "<br>";
			int i = 0;
            for (i = 0; i <= splitout.GetUpperBound(0); i++)
            {
				result += "<b>Split </b>" + i + 1 + ") " + splitout[i] + "<br>";
			}
			filestream.Close();

			return result;
		}

	}
}


 
