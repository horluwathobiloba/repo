using Onyx.AuthService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Entities;
using System.Threading;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthResult> Login(string username, string password, int organizationId);
        //Task<AuthResult> GeoLocationLogin(GeoLocationLogin geoLocationLogin, int organizationId);
        Task<AuthResult> ApplicationLogin(string name, CancellationToken cancellationToken);

        Task<Result> ResetPassword(string userId, string password);
        Task<Result> LogOut(string username);

        Task<Result> ChangePassword(string userName, string oldPassword, string newPassword);
        Task<Result> ForgotPassword(string email);

       // Task<Result> SendVerificationEmail(string email);

    }
}
