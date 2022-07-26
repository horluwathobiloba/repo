using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, string userName)> GetUserNameAsync(string userId);
        Task<(Result Result, User user)> GetUserById(string userId);
        Task<(Result Result, User user)> GetUserByRoleId(int roleId);
        
        Task<(Result Result, User user)> GetUserByEmail(string email);
        Task<(Result Result, User user)> GetUserByIdAndSubscriber(string userId, int SubscriberId);
        Task<(Result Result, User user)> GetUserByIdSubscriberAndPermissions(string userId, int SubscriberId, int accessLevelId);
        Task<(Result Result, User user)> GetUserByUsername(string userName);
        Task<(Result Result, string UserId)> CreateUserAsync(User user);
        Task<Result> VerifyEmailAsync(User user, string token);
        Task<Result> UpdateUserAsync(User user);
        Task<Result> DeleteUserAsync(string userId);
        Task<(Result result, List<User> users)> GetAll(int take, int skip);
        Task<(Result result, List<User> users)> GetUsersBySubscriberId(int SubscriberId, int take, int skip);
        Task<Result> CreateUsersAsync(List<User> user);

        Task<(bool success, string token)> GenerateEmailToken(string email);
        Task<Result> ChangeUserStatusAsync(User user);
    }
}
