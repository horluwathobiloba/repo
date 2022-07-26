using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Infrastructure.Auth;
using Onyx.WorkFlowService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Infrastructure.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;


        public AuthenticateService(ITokenService tokenService, UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
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
                var isLoginValid =  await _userManager.CheckPasswordAsync(appUser,password);
                if (isLoginValid)
                {
                    authResult.Message = "User Login was successful";
                    authResult.IsSuccess = true;
                    var role = await _context.Roles.FirstOrDefaultAsync(a=>a.Id == appUser.RoleId);
                    var rolePermissions = await _context.RolePermissions.Where(a => a.RoleId == appUser.RoleId).ToListAsync();
                    var staff = new Staff
                    {

                        StaffId = appUser.StaffId,
                        OrganizationId = appUser.OrganizationId,
                        RoleId = appUser.RoleId,
                        Role = appUser.Role,
                        FirstName = appUser.FirstName,
                        MiddleName = appUser.MiddleName,
                        LastName = appUser.LastName,
                        PhoneNumber = appUser.PhoneNumber,
                        Email = appUser.UserName,
                        Address = appUser.Address,
                        Gender = appUser.Gender,
                        StaffCode = appUser.StaffCode,
                        MarketId = appUser.MarketId,
                        DepartmentId = appUser.DepartmentId,
                        SupervisorId = appUser.SupervisorId,
                        DateOfBirth = appUser.DateOfBirth,
                        EmploymentDate = appUser.EmploymentDate,
                        CreatedBy = appUser.CreatedBy,
                        CreatedDate = appUser.CreatedDate,
                        LastModifiedBy = appUser.LastModifiedBy,
                        LastModifiedDate = appUser.LastModifiedDate,
                        Status = appUser.Status,
                        LastAccessedDate = appUser.LastAccessedDate
                    };
                     authResult.Token = _tokenService.GenerateToken(staff, rolePermissions);
                    return authResult;
                }
                else
                {
                    authResult.Message = "User Login was not successful";
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

        public async Task<AuthResult> GeoLocationLogin(GeoLocationLogin geoLocationLogin, int organizationId)
        {
            var authResult = new AuthResult();
            try
            {
                var appUser = await _userManager.FindByNameAsync(geoLocationLogin.UserName);
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
                var isLoginValid = await _userManager.CheckPasswordAsync(appUser, geoLocationLogin.Password);
                if (isLoginValid)
                {
                    authResult.Message = "User Login was successful";
                    authResult.IsSuccess = true;
                    var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == appUser.RoleId);
                    var rolePermissions = await _context.RolePermissions.Where(a => a.RoleId == appUser.RoleId).ToListAsync();
                    appUser.Latitude = geoLocationLogin.Latitude;
                    appUser.Longitude = geoLocationLogin.Longitude;
                    appUser.CurrentLocationAddress = geoLocationLogin.CurrentLocationAddress;
                    var staff = new Staff
                    {

                        StaffId = appUser.StaffId,
                        OrganizationId = appUser.OrganizationId,
                        RoleId = appUser.RoleId,
                        Role = appUser.Role,
                        FirstName = appUser.FirstName,
                        MiddleName = appUser.MiddleName,
                        LastName = appUser.LastName,
                        PhoneNumber = appUser.PhoneNumber,
                        Email = appUser.UserName,
                        Address = appUser.Address,
                        Gender = appUser.Gender,
                        StaffCode = appUser.StaffCode,
                        MarketId = appUser.MarketId,
                        DepartmentId = appUser.DepartmentId,
                        SupervisorId = appUser.SupervisorId,
                        DateOfBirth = appUser.DateOfBirth,
                        EmploymentDate = appUser.EmploymentDate,
                        CreatedBy = appUser.CreatedBy,
                        CreatedDate = appUser.CreatedDate,
                        LastModifiedBy = appUser.LastModifiedBy,
                        LastModifiedDate = appUser.LastModifiedDate,
                        Status = appUser.Status,
                        LastAccessedDate = appUser.LastAccessedDate,
                        Latitude = appUser.Latitude,
                        Longitude = appUser.Longitude,
                        CurrentLocationAddress = appUser.CurrentLocationAddress

                    };
                    await _userManager.UpdateAsync(appUser);
                    authResult.Token = _tokenService.GenerateToken(staff, rolePermissions);
                    return authResult;
                }
                else
                {
                    authResult.Message = "User Login was not successful";
                    authResult.IsSuccess = false;
                    return authResult;
                }
            }
            catch (Exception ex)
            {
                authResult.Message = "Error logging in : " + ex?.Message ?? ex.InnerException.Message;
                authResult.IsSuccess = false;
                return authResult;
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

        
    }
}
