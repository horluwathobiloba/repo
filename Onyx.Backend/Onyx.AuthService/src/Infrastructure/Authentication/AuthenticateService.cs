using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Infrastructure.Auth;
using Onyx.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IStringHashingService _stringHashingService;
        private readonly IVerificationEmailService _verificationEmailService;
        private readonly IEmailService _emailService;

        public AuthenticateService(ITokenService  tokenService, UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService,
            IApplicationDbContext context, IPasswordService passwordService, IStringHashingService stringHashingService, IVerificationEmailService verificationEmailService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
            _stringHashingService = stringHashingService;
            _verificationEmailService = verificationEmailService;
            _emailService = emailService;
        }

     
        public async Task<Result> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userName);
                if (appUser == null)
                {
                    return Result.Failure(new string[] { "User does not exist with this password"});
                }
                 var result = await _userManager.ChangePasswordAsync(appUser, oldPassword, newPassword);
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

        public async Task<AuthResult> Login(string userName, string password,int organizationId)
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
                //check for organization
                if (appUser.OrganizationId != organizationId)
                {
                    authResult.Message = "User does not exist on this organization";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == appUser.RoleId);
                var staff = new User
                {

                    UserId = appUser.UserId,
                    OrganizationId = appUser.OrganizationId,
                    RoleId = appUser.RoleId,
                    Role = appUser.Role,
                    JobFunction = appUser.JobFunction,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    PhoneNumber = appUser.PhoneNumber,
                    Email = appUser.UserName,
                    Address = appUser.Address,
                    Gender = appUser.Gender,
                    UserCode = appUser.UserCode,
                    DateOfBirth = appUser.DateOfBirth,
                    EmploymentDate = appUser.EmploymentDate,
                    CreatedBy = appUser.CreatedBy,
                    CreatedDate = appUser.CreatedDate,
                    LastModifiedBy = appUser.LastModifiedBy,
                    LastModifiedDate = appUser.LastModifiedDate,
                    Status = appUser.Status,
                    LastAccessedDate = appUser.LastAccessedDate,
                    State = appUser.State,
                    Country = appUser.Country,
                    ProfilePicture = appUser.ProfilePicture
                };
                if (!appUser.EmailConfirmed)
                {
                    await _verificationEmailService.SendVerificationEmail(staff, _emailService,_configuration, _stringHashingService);
                    authResult.Message = "You are not verified. Please check your mail to verify your account";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                var isLoginValid =  await _userManager.CheckPasswordAsync(appUser,password);
                if (isLoginValid)
                {
                    authResult.Message = "User Login was successful";
                    authResult.IsSuccess = true;
                  
                    var jobFunction = await _context.JobFunctions.FirstOrDefaultAsync(a => a.Id == appUser.JobFunctionId);
                    var rolePermissions = await _context.RolePermissions.Where(a => a.RoleId == appUser.RoleId).ToListAsync();

                    authResult.Token = await _tokenService.GenerateAccessToken(staff);
                    authResult.Token.UserToken =await _tokenService.GenerateUserToken(staff, rolePermissions);
                    //create refresh token
                    var refreshToken = await _tokenService.GenerateRefreshToken(userName, password);
                    authResult.Token.RefreshToken = refreshToken.RefreshAccessToken;

                    var tokenTable = new UserRefreshToken
                    {
                        Email = appUser.Email,
                        RefreshTokenExpires = DateTime.Now.AddDays(7),
                        CreatedBy = appUser.UserId,
                        RefreshTokens = refreshToken.RefreshAccessToken,
                        Name = appUser.FirstName + " "+ appUser.LastName,
                        CreatedDate = DateTime.Now
                    };
                    //Add refreshtoken details to the db
                    var tokenParam = await _context.UserRefreshTokens.AddAsync(tokenTable);
                    await _userManager.ResetAccessFailedCountAsync(appUser);
                    await _userManager.UpdateAsync(appUser);
                    CancellationToken cancellationToken = new CancellationToken();
                    await _context.SaveChangesAsync(cancellationToken);
                    return authResult;
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
                authResult.Message = "Error logging in : "+ex?.Message + ex?.InnerException.Message;
                authResult.IsSuccess = false;
                return authResult;
            }
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
                appUser.LastAccessedDate = DateTime.Now;
                    await _userManager.UpdateAsync(appUser);
                return Result.Success("User Logout was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] {"Error logging out", ex?.Message + ex?.InnerException.Message });
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
            if (res.Succeeded)
            {

                return Result.Success("Password Reset Successful", true);
            }
            return Result.Failure("Reset Password Failed");

        }

    }
}
