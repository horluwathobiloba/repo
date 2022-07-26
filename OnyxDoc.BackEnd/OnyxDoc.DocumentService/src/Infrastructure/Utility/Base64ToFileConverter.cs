using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
    public class Base64ToFileConverter : IBase64ToFileConverter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Base64ToFileConverter(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public string ConvertBase64StringToFile(string base64String, string fileName)
        {
            var filePath = "";
            try
            {
                if (!string.IsNullOrEmpty(base64String))
                {
                    var directory = _environment.WebRootPath;
                    if (directory == null)
                    {
                        directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot");

                    }
                    var uploadDirectory = directory + "\\ContractTemplates\\";
                    //handle Azure VM and Kubernetes scenarios
                    if (base64String.Contains(_configuration["FileLocation:Images"]))
                    {
                        return base64String;
                    }

                    if (!Directory.Exists(uploadDirectory))
                    {
                        uploadDirectory = _configuration["FileLocation:Images"];
                    }
                    //Kubernetes
                    if (base64String.Contains(_environment.ContentRootPath + "wwwroot/images/"))
                    {
                        return base64String;
                    }
                    if (!Directory.Exists(uploadDirectory))
                    {
                        uploadDirectory = _environment.ContentRootPath + "wwwroot/images/";
                    }
                    //var uploadDirectory = configuration["FileLocation:FileWrite"];
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }
                    var imageBytes = Convert.FromBase64String(base64String);
                    if (imageBytes.Length > 0)
                    {
                        string file = Path.Combine(uploadDirectory, fileName);
                        if (imageBytes.Length > 0)
                        {
                            using (var stream = new FileStream(file, FileMode.Create))
                            {
                                stream.Write(imageBytes, 0, imageBytes.Length);
                                stream.Flush();
                            }
                            filePath = fileName;
                        }
                    }
                }

                return filePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string RetrieveFileUrl(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var directory = _configuration["FileLocation:Images"];
                if (fileName.Contains(_configuration["FileLocation:Images"]))
                {
                    return fileName;
                }
                if (fileName.Contains(_environment.ContentRootPath + "wwwroot/images/"))
                {
                    return fileName;
                }
                fileName = fileName.Replace("\\images\\", "");
                fileName = fileName.Replace("C:\\inetpub\\wwwroot\\Ruby.LoanService\\wwwroot", "");
                return directory + fileName;
            }
            return "";
        }

        public void ReadWordDocument(string filepath)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
        new DataColumn("LastName"), new DataColumn("FatherName"), new DataColumn("Adress"), new DataColumn("Name"), new DataColumn("Birthday") });
            dt.Rows.Add("Pulodov", "Abdulloevich", "city Dushanbe", "Rustam", "22.12.1987");
            if (dt.Rows.Count > 0)
            {

                var directory = _configuration["FileLocation:Images"];

                var webPath = _environment.WebRootPath;
                object fileName = webPath + "~/Files/Test.doc";
                Word.Application word = new Word.Application();
                Word.Document doc = new Word.Document();
                object missing = System.Type.Missing;
                try
                {
                    doc = word.Documents.Open(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    doc.Activate();
                    foreach (Microsoft.Office.Interop.Word.Range docRange in doc.Words)
                    {
                        if (docRange.Text.Trim() == "#")
                        {
                            docRange.Text = docRange.Text.Replace("#", "");
                        }
                        else if (docRange.Text.Trim() == "Ln")
                        {
                            docRange.Text = docRange.Text.Replace("Ln", dt.Rows[0]["LastName"].ToString());
                        }
                        else if (docRange.Text.Trim() == "Fn")
                        {
                            docRange.Text = docRange.Text.Replace("Fn", dt.Rows[0]["FatherName"].ToString());
                        }
                        else if (docRange.Text.Trim() == "Ad")
                        {
                            docRange.Text = docRange.Text.Replace("Ad", dt.Rows[0]["Adress"].ToString());
                        }
                        else if (docRange.Text.Trim() == "Nm")
                        {
                            docRange.Text = docRange.Text.Replace("Nm", dt.Rows[0]["Name"].ToString());
                        }
                        else if (docRange.Text.Trim() == "Bth")
                        {
                            docRange.Text = docRange.Text.Replace("Bth", dt.Rows[0]["Birthday"].ToString());
                        }
                    }
                    doc.SaveAs(@"C:\Users\Test\Desktop\" + dt.Rows[0]["Name"].ToString() + ".doc", missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    ((Word._Application)word).Quit();
                }
            }
        }

        public bool SaveImage(string ImgStr, string fileName, string extension)
        {
            //string contentRootPath = _environment.ContentRootPath;
            //string webRootPath = _environment.WebRootPath;
            String path = _environment.WebRootPath + "/ImageStorage";

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = fileName + extension;
            //set the image path
            string imgPath = Path.Combine(path, imageName);
            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }
    }


}
