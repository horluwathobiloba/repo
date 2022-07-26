using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;

namespace ReventInject.Utilities
{

    public static class HTMLHelper
    {
        /// <summary>This provides a simple way to convert a DataTable to an HTML file. </summary>
        /// <param name="dt">This the table to convert.</param>
        /// <returns>This is the HTML output, which can saved as a file.</returns>
        public static string ConvertToHtmlPage(this DataTable dt)
        {
            string myHtmlFile = "";

            try
            {

                if ((dt == null))
                {
                    throw new System.ArgumentNullException("Data table is empty");
                }
                else
                {
                    //Continue.
                }


                //Get a worker object.
                System.Text.StringBuilder myBuilder = new System.Text.StringBuilder();


                //Open tags and write the top portion.
                myBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
                myBuilder.Append("<head>");
                myBuilder.Append("<title>");
                myBuilder.Append("Page-");
                myBuilder.Append(Guid.NewGuid().ToString());
                myBuilder.Append("</title>");
                myBuilder.Append("</head>");
                myBuilder.Append("<body>");
                myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
                myBuilder.Append("style='border: solid 1px Silver; font-size: x-small;'>");


                //Add the headings row.


                myBuilder.Append("<tr align='left' valign='top'>");


                foreach (DataColumn myColumn in dt.Columns)
                {
                    myBuilder.Append("<td align='left' valign='top'>");
                    myBuilder.Append(myColumn.ColumnName);
                    myBuilder.Append("</td>");
                }


                myBuilder.Append("</tr>");


                //Add the data rows.
                foreach (DataRow myRow in dt.Rows)
                {
                    myBuilder.Append("<tr align='left' valign='top'>");


                    foreach (DataColumn myColumn in dt.Columns)
                    {
                        myBuilder.Append("<td align='left' valign='top'>");
                        myBuilder.Append(myRow[myColumn.ColumnName].ToString());
                        myBuilder.Append("</td>");
                    }


                    myBuilder.Append("</tr>");
                }


                //Close tags.
                myBuilder.Append("</table>");
                myBuilder.Append("</body>");
                myBuilder.Append("</html>");


                //Get the string for return.
                myHtmlFile = myBuilder.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return myHtmlFile;
        }


        /// <summary>This provides a simple way to convert a DataTable to an HTML file.</summary>
        /// <param name="dt">This the table to convert.</param>
        /// <returns>This is the HTML output, which can saved as a file.</returns>
        public static string ConvertToHtmlTable(this DataTable dt)
        {
            string myHtmlFile = "";

            try
            {


                if ((dt == null))
                {
                    throw new System.ArgumentNullException("Data table is empty");
                }
                else
                {
                    //Continue.
                }


                //Get a worker object.
                System.Text.StringBuilder myBuilder = new System.Text.StringBuilder();


                //Open tags and write the top portion.

                myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
                myBuilder.Append("style='border: solid 1px Silver; font-size: x-small;'>");

                //Add the headings row.

                myBuilder.Append("<tr align='left' valign='top'>");

                foreach (DataColumn myColumn in dt.Columns)
                {
                    myBuilder.Append("<td align='left' valign='top'>");
                    myBuilder.Append(myColumn.ColumnName);
                    myBuilder.Append("</td>");
                }

                myBuilder.Append("</tr>");


                //Add the data rows.
                foreach (DataRow myRow in dt.Rows)
                {
                    myBuilder.Append("<tr align='left' valign='top'>");

                    foreach (DataColumn myColumn in dt.Columns)
                    {
                        myBuilder.Append("<td align='left' valign='top'>");
                        myBuilder.Append(myRow[myColumn.ColumnName].ToString());
                        myBuilder.Append("</td>");
                    }

                    myBuilder.Append("</tr>");
                }

                //Close tags.
                myBuilder.Append("</table>");

                //Get the string for return.
                myHtmlFile = myBuilder.ToString();



            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return myHtmlFile;
        }

    }

}
 