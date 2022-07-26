using Onyx.WorkFlowService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Entities;

namespace Onyx.WorkFlowService.Application.Common.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthResult> Login(string username, string password, int organizationId);
        Task<AuthResult> GeoLocationLogin(GeoLocationLogin geoLocationLogin, int organizationId);

        Task<Result> LogOut(string username);

        Task<Result> ChangePassword(string userName, string oldPassword, string newPassword);

       // Task<Result> SendVerificationEmail(string email);

    }
}
