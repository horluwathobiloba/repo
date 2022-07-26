using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordToPDF;

namespace Onyx.ContractService.Infrastructure.Utility
{
    public class WordUtilities
    {

        public string ConvertWordToPDF()
        {
            Word2Pdf objWorPdf = new Word2Pdf();
            string backfolder1 = "D:\\WOrdToPDF\\";
            string strFileName = "TestFile.docx";
            object FromLocation = backfolder1 + "\\" + strFileName;
            string FileExtension = Path.GetExtension(strFileName);
            string ChangeExtension = strFileName.Replace(FileExtension, ".pdf");
            if (FileExtension == ".doc" || FileExtension == ".docx")
            {
                object ToLocation = backfolder1 + "\\" + ChangeExtension;
                objWorPdf.InputLocation = FromLocation;
                objWorPdf.OutputLocation = ToLocation;
               var obj =  objWorPdf.Word2PdfCOnversion();
            }
            throw new Exception("Document must be in word format.");
        }

    }
}
