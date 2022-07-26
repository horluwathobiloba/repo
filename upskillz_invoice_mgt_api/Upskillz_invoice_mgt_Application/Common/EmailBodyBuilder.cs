using System.IO;
using System.Threading.Tasks;

namespace Upskillz_invoice_mgt_Application.Common
{
    public class EmailBodyBuilder
    {
        public static async Task<string> GetEmailBody(string emailTempPath,string companyName, string companyId, string email)
        {
            var link = $"https://upskillz.com/";
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var emailBody = temp.Replace("**link**", link).Replace("**CompanyName**", companyName);
            return emailBody;
        }
    }
}
