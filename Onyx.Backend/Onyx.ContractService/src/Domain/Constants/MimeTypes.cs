using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Onyx.ContractService.Domain.Constants
{
    public class MimeTypes
    {
        public const string Pdf = "application/pdf";
        public const string Word = "application/msword";
        public const string Png = "image/png";
        public const string Jpeg = "image/jpeg";
        public const string Jpg = "image/jpg";

        public static IDictionary<string, string> GetMimeTypes()
        { 
            var list = new Dictionary<string, string>();
            list.Add("pdf", Pdf);
            list.Add("Word", Word);
            list.Add("Png", Png);
            list.Add("Jpeg", Jpeg);
            list.Add("Jpg", Jpg);

            return list;
        }

    } 
}
