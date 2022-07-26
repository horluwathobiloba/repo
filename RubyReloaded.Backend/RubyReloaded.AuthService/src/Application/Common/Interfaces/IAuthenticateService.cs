using RubyReloaded.AuthService.Application.Common.Models;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Domain.Entities;
using System.Threading;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthResult> Login(string email, string password, List<int> cooperativeIds, List<int> ajoids);
        //Task<AuthResult> GeoLocationLogin(GeoLocationLogin geoLocationLogin, int organizationId);
        Task<AuthResult> ApplicationLogin(string name, CancellationToken cancellationToken);

        Task<Result> ResetPassword(string userId, string password);
        Task<Result> LogOut(string username);

        Task<Result> ChangePassword(string userName, string oldPassword, string newPassword);
        Task<Result> ForgotPassword(string email);

    }
}
