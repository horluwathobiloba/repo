using OnyxDoc.AuthService.Application.Common.Models;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Entities;
using System.Threading;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthResult> Login(string username, string password);
        Task<AuthResult> LoginWithThirdParty(string email, ThirdPartyType thirdParyType);
        //Task<AuthResult> GeoLocationLogin(GeoLocationLogin geoLocationLogin, int SubscriberId);
        Task<AuthResult> ApplicationLogin(string name, CancellationToken cancellationToken);

        Task<Result> ResetPassword(string userId, string password);
        Task<Result> LogOut(string username);

        Task<Result> ChangePassword(string email, string oldPassword, string newPassword);
        Task<Result> ForgotPassword(string email);

       // Task<Result> SendVerificationEmail(string email);

    }
}
