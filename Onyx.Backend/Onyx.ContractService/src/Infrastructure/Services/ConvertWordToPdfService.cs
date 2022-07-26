using Onyx.ContractService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.IO;


namespace Onyx.ContractService.Infrastructure.Services
{
  //  public class ConvertWordToPdfService : IConvertWordToPdfService
   // {
        //        public Task<string> ConvertWordToPdf(string fileName)
        //        {
        //            if (Request.Form.Files != null)
        //            {
        //                // Gets the extension from file
        //                string extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
        //                // Compares extension with supported extensions
        //                if (extension == ".doc" || extension == ".docx" || extension == ".dot" || extension == ".dotx" || extension == ".dotm" || extension == ".docm"
        //                   || extension == ".xml" || extension == ".rtf")
        //                {
        //                    MemoryStream stream = new MemoryStream();
        //                    // Retrieves the document stream
        //                    Request.Form.Files[0].CopyTo(stream);
        //                    try
        //                    {
        //                        //Creates new instance of WordDocument
        //                        WordDocument document = new WordDocument(stream, FormatType.Automatic);
        //                        stream.Dispose();
        //                        stream = null;
        //                        // Creates new instance of DocIORenderer for Word to PDF conversion
        //                        DocIORenderer render = new DocIORenderer();
        //                        // Converts Word document into PDF document
        //                        PdfDocument pdf = render.ConvertToPDF(document);
        //                        // Saves the converted PDF file
        //                        MemoryStream memoryStream = new MemoryStream();
        //                        pdf.Save(memoryStream);
        //                        // Releases all resources used by the DocIORenderer and WordDocument instance
        //                        render.Dispose();
        //                        document.Close();
        //                        // Releases all resources used by the PdfDocument instance
        //                        pdf.Close();
        //                        memoryStream.Position = 0;

        //                        return File(memoryStream, "application/pdf", "WordToPDF.pdf");
        //                    }
        //        }
        //    }
        //}
   // }
}
