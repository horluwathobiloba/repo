using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, string userName)> GetUserNameAsync(string userId);
        Task<Result> GetUsersDashboardAsync();
        Task<(Result Result, Domain.Entities.User user)> GetUserById(string userId);
        Task<(Result Result, Domain.Entities.User user)> GetUserByEmail(string email);
        Task<(Result Result, Domain.Entities.User user)> GetUserByUsername(string userName);
        Task<(Result Result, string UserId)> CreateUserAsync(Domain.Entities.User user);
        Task<Result> VerifyEmailAsync(Domain.Entities.User user);
        Task<(bool success, string token,string reason)> GenerateOTP(string email,string reason);
        Task<(bool success, string message)> ValidateOTP(string otp, string email);
        Task<Result> UpdateUserAsync(Domain.Entities.User user);
        Task<(Result result, List<Domain.Entities.User> users)> GetAll(int skip, int take);
        Task<(Result result, List<Domain.Entities.User> users)> GetAllSystemOwners(int skip, int take);
        Task<(bool success, string token)> GenerateEmailToken(string email);
        Task<(bool success, AjoCode ajoCode, string message)> GenerateAjoCode(int ajoid, string email);
        Task<(bool success, List<AjoCode> ajoCodes, string message)> GenerateAjoCodes(int ajoid, List<string> emails);
        Task<(bool success, CooperativeUserCode coopCode, string message)> GenerateCooperativeCode(int cooperativeid, string email);
        Task<(bool success, List<CooperativeUserCode> coopCodes, string message)> GenerateCooperativeCodes(int cooperativeid, List<string> emails);
        Task<Result> ChangeUserStatusAsync(Domain.Entities.User user);
        Task<Result> ChangeUserNotificationStatusAsync(string userId);
        Task<Result> ChangeTransactionPin(string userId,string pin);
        Task<Result> UpdateBVN(string userid,string bvn);
        Task<Result> ChangeUserPhoneNumber(string email, string phoneNumber);
        Task<Result> VerifyTransactionPin(string userId, string pin);
    }
}
