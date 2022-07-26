using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Infrastructure.Auth;
using RubyReloaded.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Infrastructure.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        public AuthenticateService(ITokenService  tokenService, UserManager<ApplicationUser> userManager, IApplicationDbContext context, 
            IPasswordService passwordService, IConfiguration configuration, IEmailService emailService,IIdentityService identityService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
            _emailService = emailService;
            _identityService = identityService;
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

        public async Task<AuthResult> Login(string email, string password,List<int> cooperativeIds, List<int> ajoids)
        {
            var authResult = new AuthResult();
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                //we need to talk about how to validate admins
                if (appUser == null)
                {
                    authResult.Message = "User does not exist";
                    authResult.IsSuccess = false;
                    authResult.Verified = false;
                    return authResult;
                }

                if (appUser.Verified==false)
                {
                 
                    authResult.Message = "Please Verify Your Email";
                    authResult.IsSuccess = false;
                    authResult.Verified = false;
                    var sendOtpRequest = new CreateOTPCommand
                    {
                        Email = email,
                        Reason = "Email Verification"
                    };
                    var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService, _configuration).Handle(sendOtpRequest, new CancellationToken());
                    if (!handler.Succeeded)
                    {
                        authResult.Message = "Please Verify Your Email";
                        authResult.IsSuccess = false;
                        authResult.Verified = false;
                    }
                    return authResult;
                }
                ////check for organization
                //if (appUser.OrganizationId != organizationId)
                //{
                //    authResult.Message = "User does not exist on this organization";
                //    authResult.IsSuccess = false;
                //    return authResult;
                //}
                //if (!appUser.EmailConfirmed)
                //{
                //    authResult.Message = "You are not verified. Please check your mail to verify your account";
                //    authResult.IsSuccess = false;
                //    return authResult;
                //}
                var isLoginValid =  await _userManager.CheckPasswordAsync(appUser,password);
                if (isLoginValid)
                {
                    authResult.Message = "User Login was successful";
                    authResult.IsSuccess = true;
                    authResult.Verified = true;
                    var staff = new User
                    {
                        UserId = appUser.UserId,
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName,
                        PhoneNumber = appUser.PhoneNumber,
                        Email = appUser.Email,
                        UserName=appUser.UserName,
                        Address = appUser.Address,
                        DateOfBirth = appUser.DateOfBirth,
                        CreatedBy = appUser.CreatedBy,
                        CreatedDate = appUser.CreatedDate,
                        LastModifiedBy = appUser.LastModifiedBy,
                        LastModifiedDate = appUser.LastModifiedDate,
                        Status = appUser.Status,
                        LastAccessedDate = appUser.LastAccessedDate,
                        State = appUser.State,
                        Country = appUser.Country,
                        ProfilePicture = appUser.ProfilePicture,
                        TransactionPin=appUser.TransactionPin,
                        BVN=appUser.BVN,
                        Gender=appUser.Gender
                    };
                    if (!string.IsNullOrEmpty(staff.BVN)&&!string.IsNullOrEmpty(staff.TransactionPin))
                    {
                        authResult.OnBoardingStatus = true;
                    }
                    else
                    {
                        authResult.OnBoardingStatus = false;
                    }

                    authResult.Token = await _tokenService.GenerateAccessToken(staff);
                    authResult.Token.UserToken =await _tokenService.GenerateUserToken(staff);
                   
                    authResult.Cooperatives = cooperativeIds;
                    authResult.Ajos = ajoids;
                    await _userManager.ResetAccessFailedCountAsync(appUser);
                    await _userManager.UpdateAsync(appUser);
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
                authResult.Message = "Error logging in : "+ex?.Message ?? ex.InnerException.Message;
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
