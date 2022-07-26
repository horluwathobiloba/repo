using System.Threading.Tasks;

namespace Upskillz_invoice_mgt_Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<string>> GetUserNameAsync(string userId);
        Task<Response<bool>> ValidateUser(string email , string password);
    }
}
