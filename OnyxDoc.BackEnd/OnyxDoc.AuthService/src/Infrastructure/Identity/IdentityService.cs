using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Entities;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace OnyxDoc.AuthService.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, IBase64ToFileConverter base64ToFileConverter, IConfiguration configuration, IApplicationDbContext context)
        {
            _userManager = userManager;
            _base64ToFileConverter = base64ToFileConverter;
            _configuration = configuration;
            _context = context;
        }


        public async Task<(Result Result, string UserId)> CreateUserAsync(User user)
        {
            try
            {
                var userDetails = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedById = user.CreatedById,
                    CreatedByEmail = user.CreatedByEmail,
                    CreatedDate = user.CreatedDate,
                    SubscriberId = user.SubscriberId,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    UserName = user.Email,
                    Status = Domain.Enums.Status.Inactive,
                    Country = user.Country,
                    City = user.City,
                    EmailConfirmed = false,
                    ProfilePicture = user.ProfilePicture,
                    Token = user.Token,
                    UserCreationStatus = user.UserCreationStatus,
                    UserCreationStatusDesc = user.UserCreationStatusDesc
                };
                var result = await _userManager.CreateAsync(userDetails, user.Password);
                if (!result.Succeeded)
                {
                    return (Result.Failure(new string[] { result.Errors.FirstOrDefault().Description }), "");
                }
                //update User Id
                userDetails.UserId = userDetails.Id;
                await _userManager.UpdateAsync(userDetails);
                return (result.ToApplicationResult(), userDetails.Id);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error creating user", ex?.Message ?? ex?.InnerException?.Message }), "");
            }
        }

        public async Task<Result> CreateUsersAsync(List<User> user)
        {
            try
            {
                var userDetails = new List<ApplicationUser>();

                foreach (var item in user)
                {
                    var entity = new ApplicationUser
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        CreatedById = item.CreatedById,
                        CreatedByEmail = item.CreatedByEmail,
                        CreatedDate = item.CreatedDate,
                        SubscriberId = item.SubscriberId,
                        PhoneNumber = item.PhoneNumber,
                        RoleId = item.RoleId,
                        UserName = item.Email,
                        Status = Domain.Enums.Status.Inactive,
                        Country = item.Country,
                        City = item.City,
                        EmailConfirmed = false,
                        ProfilePicture = item.ProfilePicture,
                        Token = item.Token,
                        UserCreationStatus = item.UserCreationStatus,
                        UserCreationStatusDesc = item.UserCreationStatusDesc
                        
                    };
                    userDetails.Add(entity);
                    var result = await _userManager.CreateAsync(entity, item.Password);
                    CancellationToken token = new CancellationToken();
                    await _context.SaveChangesAsync(token);
                    if (!result.Succeeded)
                    {
                        //return (Result.Failure(result.Errors.First().Description), new List<User>());
                        return (Result.Failure(result.Errors.First().Description));
                    }
                    //update User Id
                    item.UserId = entity.Id;
                    await _userManager.UpdateAsync(entity);

                }
                return (Result.Success());
            }
            catch (Exception ex)
            {
                // return (Result.Failure(new string[] { ex.Message }), new List<User>());
                return (Result.Failure(new string[] { ex.Message }));
            }
        }

        public async Task<Result> ChangeUserStatusAsync(User user)
        {
            try
            {

                var appUser = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == user.UserId);
                string message = "";
                if (appUser != null)
                {
                    switch (appUser.Status)
                    {
                        case Domain.Enums.Status.Active:
                            appUser.Status = Domain.Enums.Status.Inactive;
                            message = "User deactivation was successful";
                            break;
                        case Domain.Enums.Status.Inactive:
                            appUser.Status = Domain.Enums.Status.Active;
                            message = "User activation was successful";
                            break;
                        case Domain.Enums.Status.Deactivated:
                            appUser.Status = Domain.Enums.Status.Active;
                            message = "User activation was successful";
                            break;
                        default:
                            break;
                    }
                };
                return await ChangeUserStatusAsync(appUser, message);

            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error changing user status", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<Result> ChangeUserStatusAsync(ApplicationUser user, string message)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                CancellationToken token = new CancellationToken();
                await _context.SaveChangesAsync(token);
                if (result.Succeeded)
                    return Result.Success(message);
                else
                    return Result.Failure(result.ToApplicationResult().Messages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Result> UpdateUserAsync(User user)
        {
            try
            {
                var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.UserId);
                if (userToUpdate == null)
                {
                    return Result.Failure(new string[] { "User for update does not exist" });
                }
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Email = user.Email;
                userToUpdate.CreatedById = user.CreatedById;
                userToUpdate.CreatedByEmail = user.CreatedByEmail;
                userToUpdate.CreatedDate = user.CreatedDate;
                userToUpdate.SubscriberId = user.SubscriberId;
                userToUpdate.PhoneNumber = user.PhoneNumber;
                userToUpdate.RoleId = user.RoleId;
                userToUpdate.Status = user.Status;
                userToUpdate.Country = user.Country;
                userToUpdate.City = user.City;
                userToUpdate.ProfilePicture = user.ProfilePicture;
                userToUpdate.Signature = user.Signature;
                userToUpdate.JobTitle = user.JobTitle;
                userToUpdate.UserCreationStatus = user.UserCreationStatus;
                userToUpdate.UserCreationStatusDesc = user.UserCreationStatusDesc;
                //update password
                var token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
                var res = await _userManager.ResetPasswordAsync(userToUpdate, token, user.Password);
                var result = await _userManager.UpdateAsync(userToUpdate);
                CancellationToken cancelToken = new CancellationToken();
                await _context.SaveChangesAsync(cancelToken);
                return Result.Success("User was updated successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating user", ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        public async Task<Result> DeleteUserAsync(string userId)
        {
            try
            {
                var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userToUpdate != null)
                {
                    await _userManager.DeleteAsync(userToUpdate);
                }
              
                return Result.Success("User was deleted successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error deleting user", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<(Result result, List<User> users)> GetAll(int skip, int take)
        {

            List<User> users = new List<User>();
            try
            {
                List<ApplicationUser> appUsers = new List<ApplicationUser>();
                if (skip == 0 || take == 0)
                {
                    appUsers = await _userManager.Users.ToListAsync();
                }
                else
                {
                    appUsers = await _userManager.Users.Skip(skip)
                                     .Take(take).ToListAsync();
                }
               
              
                if (appUsers == null)
                {
                    return (Result.Failure(new string[] { "No users exist on the system" }), null);
                }

                foreach (var appUser in appUsers)
                {
                    User user = new User();
                    user.UserId = appUser.Id;
                    user.CreatedById = appUser.CreatedById;
                    user.CreatedByEmail = appUser.CreatedByEmail;
                    user.CreatedDate = appUser.CreatedDate;
                    user.Email = appUser.Email;
                    user.FirstName = appUser.FirstName;
                    user.LastLoginDate = appUser.LastLoginDate;
                    user.LastName = appUser.LastName;
                    user.Name = appUser.FirstName + " " + " " + appUser?.LastName ?? " ";
                    user.SubscriberId = appUser.SubscriberId;
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.RoleId = appUser.RoleId;
                    user.Status = appUser.Status;
                    user.Signature = appUser.Signature;
                    user.JobTitle = appUser.JobTitle;
                    user.ProfilePicture = appUser.ProfilePicture;
                    user.LastModifiedById = appUser.LastModifiedById;
                    user.LastModifiedByEmail = appUser.LastModifiedByEmail;
                    user.LastModifiedDate = appUser.LastModifiedDate;
                    user.Country = appUser.Country;
                    user.City = appUser.City;
                    user.UserCreationStatus = appUser.UserCreationStatus;
                    user.UserCreationStatusDesc = appUser.UserCreationStatusDesc;
                    user.EmailConfirmed = appUser.EmailConfirmed;
                    users.Add(user);
                }
                return (Result.Success(), users);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving users", ex?.Message ?? ex?.InnerException?.Message }), users);
            }
        }

        public async Task<(Result Result, User user)> GetUserById(string userId)
        {
            try
            {
                // var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var existingUser = await _userManager.FindByIdAsync(userId);
                if (existingUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with Id: {userId}" }), null);
                }
                var user = new User
                {
                    UserId = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.UserName,
                    CreatedById = existingUser.CreatedById,
                    CreatedByEmail = existingUser.CreatedByEmail,
                    CreatedDate = existingUser.CreatedDate,
                    SubscriberId = existingUser.SubscriberId,
                    PhoneNumber = existingUser.PhoneNumber,
                    RoleId = existingUser.RoleId,
                    Signature = existingUser.Signature,
                    JobTitle = existingUser.JobTitle,
                    Status = existingUser.Status,
                    Country = existingUser.Country,
                    City = existingUser.City,
                    ProfilePicture = existingUser.ProfilePicture,
                    UserCreationStatus = existingUser.UserCreationStatus,
                    UserCreationStatusDesc = existingUser.UserCreationStatusDesc,
                    EmailConfirmed = existingUser.EmailConfirmed
                };

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving user", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, User user)> GetUserByRoleId(int roleId)
        {
            try
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.RoleId == roleId);
                if (existingUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with role Id: {roleId}" }), null);
                }
                var user = new User
                {
                    UserId = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.UserName,
                    CreatedById = existingUser.CreatedById,
                    CreatedByEmail = existingUser.CreatedByEmail,
                    CreatedDate = existingUser.CreatedDate,
                    SubscriberId = existingUser.SubscriberId,
                    PhoneNumber = existingUser.PhoneNumber,
                    RoleId = existingUser.RoleId,
                    Status = existingUser.Status,
                    Country = existingUser.Country,
                    City = existingUser.City,
                    Signature = existingUser.Signature,
                    JobTitle = existingUser.JobTitle,
                    ProfilePicture = existingUser.ProfilePicture,
                    UserCreationStatus = existingUser.UserCreationStatus,
                    UserCreationStatusDesc = existingUser.UserCreationStatusDesc,
                    EmailConfirmed = existingUser.EmailConfirmed
                };

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving user", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, User user)> GetUserByUsername(string userName)
        {
            try
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (existingUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with username: {userName}" }), null);
                }
                var user = new User
                {
                    UserId = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.UserName,
                    CreatedById = existingUser.CreatedById,
                    CreatedByEmail = existingUser.CreatedByEmail,
                    CreatedDate = existingUser.CreatedDate,
                    SubscriberId = existingUser.SubscriberId,
                    PhoneNumber = existingUser.PhoneNumber,
                    RoleId = existingUser.RoleId,
                    Status = existingUser.Status,
                    Country = existingUser.Country,
                    City = existingUser.City,
                    ProfilePicture = existingUser.ProfilePicture,
                    Signature = existingUser.Signature,
                    JobTitle = existingUser.JobTitle,
                    UserCreationStatus = existingUser.UserCreationStatus,
                    UserCreationStatusDesc = existingUser.UserCreationStatusDesc,
                    EmailConfirmed = existingUser.EmailConfirmed
                };

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving user by username", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, string userName)> GetUserNameAsync(string userId)
        {
            try
            {
                var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

                return (Result.Success(), user.UserName);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), "");
            }
        }

        public async Task<(Result Result, User user)> GetUserByIdAndSubscriber(string userId, int SubscriberId)
        {
            var user = new User();
            try
            {

                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.SubscriberId == SubscriberId && u.Id == userId);
                if (existingUser != null)
                {
                    user = new User
                    {
                        UserId = existingUser.Id,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        Email = existingUser.Email,
                        CreatedById = existingUser.CreatedById,
                        CreatedByEmail = existingUser.CreatedByEmail,
                        CreatedDate = existingUser.CreatedDate,
                        SubscriberId = existingUser.SubscriberId,
                        PhoneNumber = existingUser.PhoneNumber,
                        RoleId = existingUser.RoleId,
                        Status = existingUser.Status,
                        Country = existingUser.Country,
                        City = existingUser.City,
                        ProfilePicture = existingUser.ProfilePicture,
                        Signature = existingUser.Signature,
                        JobTitle = existingUser.JobTitle,
                        UserCreationStatus = existingUser.UserCreationStatus,
                        UserCreationStatusDesc = existingUser.UserCreationStatusDesc,
                        EmailConfirmed = existingUser.EmailConfirmed
                    };
                }

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), user);
            }
        }

        public async Task<(Result Result, User user, Subscriber subscriber, Role role)> GetUserOrgAndRoles(string userId, int SubscriberId)
        {
            var user = new User();
            var subscriber = new Subscriber();
            var role = new Role();
            try
            {

                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.SubscriberId == SubscriberId && u.Id == userId);
                if (existingUser != null)
                {
                    user = new User
                    {
                        UserId = existingUser.Id,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        Email = existingUser.Email,
                        CreatedById = existingUser.CreatedById,
                        CreatedByEmail = existingUser.CreatedByEmail,
                        CreatedDate = existingUser.CreatedDate,
                        SubscriberId = existingUser.SubscriberId,
                        PhoneNumber = existingUser.PhoneNumber,
                        RoleId = existingUser.RoleId,
                        Status = existingUser.Status,
                        Country = existingUser.Country,
                        City = existingUser.City,
                        ProfilePicture = existingUser.ProfilePicture,
                        Signature = existingUser.Signature,
                        JobTitle = existingUser.JobTitle,
                        UserCreationStatus = existingUser.UserCreationStatus,
                        UserCreationStatusDesc = existingUser.UserCreationStatusDesc,
                        EmailConfirmed = existingUser.EmailConfirmed
                    };
                    subscriber = await _context.Subscribers.FirstOrDefaultAsync(u => u.Id == user.SubscriberId);
                    role = await _context.Roles.FirstOrDefaultAsync(u => u.Id == user.RoleId);
                }

                return (Result.Success(), user, subscriber, role);
            }
            catch (Exception ex)
            {
                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), user, subscriber, role);
            }
        }

        public async Task<(Result result, List<User> users)> GetUsersBySubscriberId(int SubscriberId, int take, int skip)
        {
            var users = new List<User>();
            try
            {
                List<ApplicationUser> appUsers = new List<ApplicationUser>();
                if (skip == 0 || take == 0)
                {
                    appUsers = await _userManager.Users.Where(u => u.SubscriberId == SubscriberId).ToListAsync();
                }
                else
                {
                    appUsers = await _userManager.Users.Where(u => u.SubscriberId == SubscriberId).Skip(skip)
                                      .Take(take).ToListAsync();
                }
               
                foreach (var appUser in appUsers)
                {
                    User user = new User();
                    user.UserId = appUser.Id;
                    user.CreatedById = appUser.CreatedById;
                    user.CreatedByEmail = appUser.CreatedByEmail;
                    user.CreatedDate = appUser.CreatedDate;
                    user.Email = appUser.Email;
                    user.FirstName = appUser.FirstName;
                    user.LastName = appUser.LastName;
                    user.SubscriberId = appUser.SubscriberId;
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.RoleId = appUser.RoleId;
                    user.Status = appUser.Status;
                    user.LastModifiedById = appUser.LastModifiedById;
                    user.LastModifiedByEmail = appUser.LastModifiedByEmail;
                    user.LastModifiedDate = appUser.LastModifiedDate;
                    user.Country = appUser.Country;
                    user.Signature = appUser.Signature;
                    user.JobTitle = appUser.JobTitle;
                    user.City = appUser.City;
                    user.ProfilePicture = appUser.ProfilePicture;
                    user.UserCreationStatus = appUser.UserCreationStatus;
                    user.UserCreationStatusDesc = appUser.UserCreationStatusDesc;
                    user.EmailConfirmed = appUser.EmailConfirmed;
                    users.Add(user);
                }
                return (Result.Success(), users);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), users);
            }
        }
        public async Task<(Result Result, User user)> GetUserByEmail(string email)
        {
            try
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (appUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with email: {email}" }), null);
                }
                var user = new User
                {
                    UserId = appUser.Id,
                    CreatedById = appUser.CreatedById,
                    CreatedByEmail = appUser.CreatedByEmail,
                    CreatedDate = appUser.CreatedDate,
                    Email = appUser.Email,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    SubscriberId = appUser.SubscriberId,
                    PhoneNumber = appUser.PhoneNumber,
                    ProfilePicture = appUser.ProfilePicture,
                    Signature = appUser.Signature,
                    JobTitle = appUser.JobTitle,
                    RoleId = appUser.RoleId,
                    Status = appUser.Status,
                    LastModifiedById = appUser.LastModifiedById,
                    LastModifiedByEmail = appUser.LastModifiedByEmail,
                    LastModifiedDate = appUser.LastModifiedDate,
                    Country = appUser.Country,
                    City = appUser.City,
                    Token = appUser.Token,
                    UserCreationStatus = appUser.UserCreationStatus,
                    UserCreationStatusDesc = appUser.UserCreationStatusDesc,
                    EmailConfirmed = appUser.EmailConfirmed
                };

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving Customer by username", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, User user)> GetUserByIdSubscriberAndPermissions(string userId, int SubscriberId, int accessLevelId)
        {
            var user = new User();
            try
            {
                var userd = await GetUserByIdAndSubscriber(userId, SubscriberId);

                //////
                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), user);
            }
        }


        public async Task<Result> VerifyEmailAsync(User user, string token)
        {
            try
            {
                var verifyUser = await _userManager.FindByEmailAsync(user.Email);
                if (verifyUser == null)
                {
                    return Result.Failure("User does not exist");
                }

                if (verifyUser.EmailConfirmed)
                {
                    return Result.Failure("Email is already confirmed");
                }
                if (verifyUser.Token != token)
                {
                    return Result.Failure("Error verifying user account, token invalid");
                }

                verifyUser.Status = Domain.Enums.Status.Active;
                verifyUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(verifyUser);
                return Result.Success("Email verified successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error verifying email", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<(bool success, string token)> GenerateEmailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "");
            }
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (emailToken != null)
            {
                return (true, emailToken);
            }
            return (false, "");
        }



    }
}

