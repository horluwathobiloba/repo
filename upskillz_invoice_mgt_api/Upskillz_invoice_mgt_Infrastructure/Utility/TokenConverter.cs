using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using Upskillz_invoice_mgt_Application.Common.Interfaces;

namespace Upskillz_invoice_mgt_Infrastructure.Utility
{
    public class TokenConverter : ITokenConverter
    {
        public string DecodeToken(string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            return Encoding.UTF8.GetString(decodedToken);
        }

        public string EncodeToken(string token)
        {
            var encodedToken = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(encodedToken);
        }
    }
}
