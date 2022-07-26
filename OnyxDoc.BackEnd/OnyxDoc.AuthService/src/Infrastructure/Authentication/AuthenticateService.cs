using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Infrastructure.Auth;
using OnyxDoc.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IVerificationEmailService _verificationEmailService;
        private readonly IEmailService _emailService;

        public AuthenticateService(ITokenService  tokenService, IIdentityService identityService, UserManager<ApplicationUser> userManager, 
                                 IApplicationDbContext context, IStringHashingService stringHashingService, IEmailService emailService, 
                                 IPasswordService passwordService, IVerificationEmailService verificationEmailService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _identityService=identityService;
            _userManager = userManager;
            _context = context;
            _passwordService = passwordService;
            _verificationEmailService = verificationEmailService;
            _stringHashingService = stringHashingService;
            _emailService = emailService;
            _configuration = configuration;
        }

     
        public async Task<Result> ChangePassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                if (appUser == null)
                {
                    return Result.Failure(new string[] { "User does not exist with this email"});
                }
                 var result = await _userManager.ChangePasswordAsync(appUser, oldPassword, newPassword);
                CancellationToken cancelToken = new CancellationToken();
                await _context.SaveChangesAsync(cancelToken);
                if (result.Succeeded)
                    return Result.Success("Change Password Successful");
                else
                    return Result.Failure(new string[] { "Change Password not Successful" });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] {"Error changing password", ex?.Message??ex.InnerException.Message});
            }
        }

        public async Task<AuthResult> Login(string userName, string password)
        {
            var authResult = new AuthResult();
            try
            {

                var appUser = await _userManager.FindByNameAsync(userName);
                if (appUser == null)
                {
                    authResult.Message = "User does not exist";
                    authResult.IsSuccess = false;
                    return authResult;
                }

                //Won't permit a deactivated user
                if (appUser.Status == Domain.Enums.Status.Deactivated || appUser.Status == Domain.Enums.Status.Inactive)
                {
                    if (!appUser.EmailConfirmed)
                    {
                        authResult.Message = "User Account is not yet verified! Please check your email for the verification email";
                        authResult.IsSuccess = false;
                        return authResult;
                    }
                    authResult.Message = "User is "+ appUser.Status.ToString();
                    authResult.IsSuccess = false;
                    return authResult;
                }
                var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == appUser.RoleId);
                var user = new User
                {

                    UserId = appUser.Id,
                    SubscriberId = appUser.SubscriberId,
                    RoleId = appUser.RoleId,
                    Role = (appUser.Role is null) ? role : appUser.Role,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    PhoneNumber = appUser.PhoneNumber,
                    Email = appUser.UserName,
                    CreatedById = appUser.CreatedById,
                    CreatedByEmail = appUser.Email,
                    CreatedDate = appUser.CreatedDate,
                    LastModifiedById = appUser.LastModifiedById,
                    LastModifiedDate = appUser.LastModifiedDate,
                    LastModifiedByEmail = appUser.LastModifiedByEmail,
                    Status = appUser.Status,
                    LastLoginDate = appUser.LastLoginDate,
                    City = appUser.City,
                    Country = appUser.Country,
                    ProfilePicture = appUser.ProfilePicture
                };
                if (!appUser.EmailConfirmed)
                {
                    return await EmailVerificationCheck(authResult, user);
                }
                var isLoginValid =  await _userManager.CheckPasswordAsync(appUser,password);
                if (isLoginValid)
                {
                    var loginResult = await SetLoginResult(authResult, appUser, user);
                    if (loginResult.IsSuccess)
                    {
                        await _userManager.ResetAccessFailedCountAsync(appUser);
                        await _userManager.UpdateAsync(appUser);
                    }
                    return loginResult;
                }
                else
                {
                    await _userManager.AccessFailedAsync(appUser);
                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        authResult.Message = "Incorrect Password! User Account has been locked out";
                        authResult.IsSuccess = false;
                        return authResult;
                    }
                    authResult.Message = "Incorrect Password! User Login was not successful";
                    authResult.IsSuccess = false;
                    return authResult;
                }
               
            }
            catch (Exception ex)
            {
                authResult.Message = "Error logging in : "+ex?.Message ?? ex.InnerException.Message;
                authResult.IsSuccess = false;
                return authResult;
            }
        }

        private async Task<AuthResult> EmailVerificationCheck(AuthResult authResult, User user)
        {
            await _verificationEmailService.SendVerificationEmail(user, _emailService, _configuration, _stringHashingService);
            authResult.Message = "You are not verified. Please check your mail to verify your account";
            authResult.IsSuccess = false;
            return authResult;
        }

        private async Task<AuthResult> SetLoginResult(AuthResult authResult, ApplicationUser appUser, User user)
        {
            authResult.Message = "User Login was successful";
            authResult.IsSuccess = true;
            Subscriber subscriber = await _context.Subscribers.Where(a => a.Id == appUser.SubscriberId).FirstOrDefaultAsync();
            if (subscriber == null)
            {
                authResult.Message = "Invalid Subscriber Details";
                authResult.IsSuccess = false;
                return authResult;
            }
            object rolePermissions = await _context.RolePermissions.Where(a => a.RoleId == appUser.RoleId).ToListAsync();
            var permissions = (List<RolePermission>)rolePermissions;
            if ((rolePermissions == null || permissions?.Count == 0))
            {
                if (subscriber.SubscriberAccessLevel == SubscriberAccessLevel.System)
                {
                    rolePermissions = await _context.Features.Where(a => a.AccessLevel == AccessLevel.All || a.AccessLevel == AccessLevel.SystemOwner).ToListAsync();
                }
                switch (subscriber.SubscriberType)
                {
                    case SubscriberType.Individual:
                        //Get Subscription plans 
                        rolePermissions = await _context.Features.Where(a => a.AccessLevel == AccessLevel.Individual ||
              a.AccessLevel == AccessLevel.IndividualAndCorporate || a.AccessLevel == AccessLevel.IndividualAndSystemOwner).ToListAsync();
                        break;
                    case SubscriberType.Corporate:
                        rolePermissions = await _context.Features.Where(a=>a.AccessLevel == AccessLevel.Corporate
                         || a.AccessLevel == AccessLevel.IndividualAndCorporate  || a.AccessLevel == AccessLevel.CorporateAndSystemOwner).ToListAsync();
                        break;
                    default:
                        break;
                }
              
            }
            var branding = await _context.Brandings.Where(a => a.SubscriberId == appUser.SubscriberId && a.Status == Status.Active).FirstOrDefaultAsync();
            authResult.Token = await _tokenService.GenerateAccessToken(user);
            authResult.Token.UserToken = await _tokenService.GenerateUserToken(subscriber.SubscriberType.ToString(), user, rolePermissions, branding);
            await GenerateAuditTrail(authResult, user);
            return authResult;
        }

        public async Task<AuthResult> LoginWithThirdParty(string email, ThirdPartyType thirdPartyType)
        {
            var authResult = new AuthResult();
            try
            {
                var appUser = await _userManager.FindByNameAsync(email);
                if (appUser == null)
                {
                    authResult.Message = "User does not exist";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == appUser.RoleId);
                var user = new User
                {

                    UserId = appUser.Id,
                    SubscriberId = appUser.SubscriberId,
                    RoleId = appUser.RoleId,
                    Role = (appUser.Role is null) ? role : appUser.Role,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    PhoneNumber = appUser.PhoneNumber,
                    Email = appUser.UserName,
                    CreatedById = appUser.CreatedById,
                    CreatedByEmail = appUser.Email,
                    CreatedDate = appUser.CreatedDate,
                    LastModifiedById = appUser.LastModifiedById,
                    LastModifiedDate = appUser.LastModifiedDate,
                    LastModifiedByEmail = appUser.LastModifiedByEmail,
                    Status = appUser.Status,
                    LastLoginDate = appUser.LastLoginDate,
                    City = appUser.City,
                    Country = appUser.Country,
                    ProfilePicture = appUser.ProfilePicture
                };
                if (!appUser.EmailConfirmed)
                {
                   return await EmailVerificationCheck(authResult, user);
                }
                authResult.ThirdPartyProvider = thirdPartyType.ToString();
                return await SetLoginResult(authResult, appUser, user);
            }
            catch (Exception ex)
            {
                authResult.Message = "Error logging in : " + ex?.Message ?? ex.InnerException.Message;
                authResult.IsSuccess = false;
                return authResult;
            }
        }

        private async Task GenerateAuditTrail(AuthResult authResult, User user)
        {
            AuditTrail auditTrail = new AuditTrail
            {
                SubscriberId = user.SubscriberId,
                AuditAction = AuditAction.LogIn,
                AuditActionDesc = AuditAction.LogIn.ToString(),
                ControllerName = "Login",
                UserId = user.UserId,
                CreatedDate = DateTime.Now,
                CreatedByEmail = user.Email,
                CreatedById = user.UserId,
                Status = Status.Active,
                StatusDesc = Status.Active.ToString(),
                RoleId = user.RoleId,
                RoleName = user?.Role?.Name
            };
            CancellationToken cancellationToken = new CancellationToken();
            await _context.AuditTrails.AddAsync(auditTrail);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<AuthResult> ApplicationLogin(string name,  CancellationToken cancellationToken)
        {

            var entity = await _context.Clients.FirstOrDefaultAsync(a => a.Name == name);
            string password = "AppLogin@onyx.com";
            string message = "";
            if (entity == null)
            {
                message = "Login does not exist";
                return new AuthResult { IsSuccess = false, Message = message,Token = null };
            }
            else
            {
                bool passwordCheck = _passwordService.VerifyPasswordHash(password, entity.PasswordHash, entity.PasswordSalt);
                if (!passwordCheck)
                {
                    message = "Incorrect Password!";
                }
                var token = await _tokenService.GenerateDeveloperToken(entity.Name, entity.Id.ToString());
                message = "Login was successful!";
                return new AuthResult { IsSuccess = true,  Message = message,Token = token };
            }
        }


        public async Task<Result> LogOut(string userName)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userName);
                if (appUser == null)
                {
                    return Result.Failure(new string[] { "User does not exist" });
                }
                appUser.LastLoginDate = DateTime.Now;
                    await _userManager.UpdateAsync(appUser);
                AuditTrail auditTrail = new AuditTrail
                {
                    SubscriberId = appUser.SubscriberId,
                    AuditAction = AuditAction.LogIn,
                    AuditActionDesc = AuditAction.LogIn.ToString(),
                    ControllerName = "Logout",
                    UserId = appUser.UserId,
                    CreatedDate = DateTime.Now,
                    CreatedByEmail = appUser.Email,
                    CreatedById = appUser.UserId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    RoleId = appUser.RoleId,
                    RoleName = appUser?.Role?.Name
                };
                CancellationToken cancellationToken = new CancellationToken();
                await _context.AuditTrails.AddAsync(auditTrail);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("User Logout was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] {"Error logging out", ex?.Message ?? ex.InnerException.Message });
            }
        }

        public async Task<Result> ForgotPassword(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                return Result.Failure(new string[] { "User does not exist with this email" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            CancellationToken cancelToken = new CancellationToken();
            await _context.SaveChangesAsync(cancelToken);
            return Result.Success("Password reset token successful", token);
        }
        public async Task<Result> ResetPassword(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result.Failure("Invalid email");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, password);
            CancellationToken cancelToken = new CancellationToken();
            await _context.SaveChangesAsync(cancelToken);
            if (res.Succeeded)
            {

                return Result.Success("Password Reset Successful", true);
            }
            return Result.Failure("Reset Password Failed");

        }

    }
}
