using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Entities;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;

namespace Onyx.AuthService.Infrastructure.Identity
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


        public async Task<(Result Result, string UserId)> CreateUserAsync(User staff)
        {
            try
            {
                var user = new ApplicationUser
                {
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.Email,
                    Address = staff.Address,
                    CreatedBy = staff.CreatedBy,
                    CreatedDate = staff.CreatedDate,
                    DateOfBirth = staff.DateOfBirth,
                    Gender = staff.Gender,
                    OrganizationId = staff.OrganizationId,
                    PhoneNumber = staff.PhoneNumber,
                    RoleId = staff.RoleId,
                    UserName = staff.Email,
                    UserCode = staff.UserCode,
                    Status = Domain.Enums.Status.Inactive,
                    EmploymentDate = staff.EmploymentDate,
                    Country = staff.Country,
                    State = staff.State,
                    EmailConfirmed = false,
                    ProfilePicture = staff.ProfilePicture,
                    JobFunctionId = staff.JobFunctionId
                };
                var result = await _userManager.CreateAsync(user, staff.Password);
                if (!result.Succeeded)
                {
                    return (Result.Failure(new string[] { result.Errors.FirstOrDefault().Description }), "");
                }
                //update User Id
                user.UserId = user.Id;
                await _userManager.UpdateAsync(user);
                return (result.ToApplicationResult(), user.Id);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), "");
            }
        }

        public async Task<Result> ChangeUserStatusAsync(User staff)
        {
            try
            {

                var appUser = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == staff.UserId);
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

        public async Task<Result> UpdateUserAsync(User staff)
        {
            try
            {
                var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == staff.UserId);
                if (userToUpdate == null)
                {
                    return Result.Failure(new string[] { "User for update does not exist" });
                }
                userToUpdate.FirstName = staff.FirstName;
                userToUpdate.LastName = staff.LastName;
                userToUpdate.Email = staff.UserName;
                userToUpdate.Address = staff.Address;
                userToUpdate.CreatedBy = staff.CreatedBy;
                userToUpdate.CreatedDate = staff.CreatedDate;
                userToUpdate.DateOfBirth = staff.DateOfBirth;
                userToUpdate.Gender = staff.Gender;
                userToUpdate.OrganizationId = staff.OrganizationId;
                userToUpdate.PhoneNumber = staff.PhoneNumber;
                userToUpdate.RoleId = staff.RoleId;
                userToUpdate.UserName = staff.UserName;
                userToUpdate.UserCode = staff.UserCode;
                userToUpdate.Status = staff.Status;
                userToUpdate.EmploymentDate = staff.EmploymentDate;
                userToUpdate.Country = staff.Country;
                userToUpdate.State = staff.State;
                userToUpdate.JobFunctionId = staff.JobFunctionId;
                userToUpdate.ProfilePicture = staff.ProfilePicture;
                return Result.Success("User was updated successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating user", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<(Result result, List<User> staffs)> GetAll()
        {

            List<User> staffs = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Include(a => a.Role).ToListAsync();
                if (appUsers == null)
                {
                    return (Result.Failure(new string[] { "No users exist on the system" }), null);
                }

                foreach (var appUser in appUsers)
                {
                    User staff = new User();
                    staff.UserId = appUser.UserId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.JobFunctionId = appUser.JobFunctionId.HasValue ? appUser.JobFunctionId.Value : 0;
                    staff.Status = appUser.Status;
                    staff.UserCode = appUser.UserCode;
                    staff.UserName = appUser.UserName;
                    staff.LastModifiedBy = appUser.LastModifiedBy;
                    staff.LastModifiedDate = appUser.LastModifiedDate;
                    staff.EmploymentDate = appUser.EmploymentDate;
                    staff.Country = appUser.Country;
                    staff.State = appUser.State;
                    staffs.Add(staff);
                }
                return (Result.Success(), staffs);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving staffs", ex?.Message ?? ex?.InnerException?.Message }), staffs);
            }
        }

        public async Task<(Result Result, User staff)> GetUserById(string userId)
        {
            try
            {
                // var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with Id: {userId}" }), null);
                }
                var staff = new User
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.UserName,
                    Address = user.Address,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    OrganizationId = user.OrganizationId,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    JobFunctionId = user.JobFunctionId.HasValue ? user.JobFunctionId.Value : 0,
                    UserName = user.UserName,
                    UserCode = user.UserCode,
                    Status = user.Status,
                    EmploymentDate = user.EmploymentDate,
                    Country = user.Country,
                    State = user.State,
                    ProfilePicture = user.ProfilePicture
                };

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving staff", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, User staff)> GetUserByUsername(string userName)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with username: {userName}" }), null);
                }
                var staff = new User
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.UserName,
                    Address = user.Address,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    OrganizationId = user.OrganizationId,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    JobFunctionId = user.JobFunctionId.HasValue ? user.JobFunctionId.Value : 0,
                    UserName = user.UserName,
                    UserCode = user.UserCode,
                    Status = user.Status,
                    EmploymentDate = user.EmploymentDate,
                    Country = user.Country,
                    State = user.State,
                    ProfilePicture = user.ProfilePicture
                };

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving staff by username", ex?.Message ?? ex?.InnerException?.Message }), null);
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

        public async Task<(Result Result, User staff)> GetUserByIdAndOrganization(string userId, int organizationId)
        {
            var staff = new User();
            try
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OrganizationId == organizationId && u.Id == userId);
                if (user != null)
                {
                    staff = new User
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.UserName,
                        Address = user.Address,
                        CreatedBy = user.CreatedBy,
                        CreatedDate = user.CreatedDate,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        OrganizationId = user.OrganizationId,
                        PhoneNumber = user.PhoneNumber,
                        RoleId = user.RoleId,
                        UserName = user.UserName,
                        UserCode = user.UserCode,
                        Status = user.Status,
                        EmploymentDate = user.EmploymentDate,
                        Country = user.Country,
                        State = user.State,
                        ProfilePicture = user.ProfilePicture
                    };
                }

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staff);
            }
        }

        public async Task<(Result Result, User staff, Organization organization, Role role)> GetUserOrgAndRoles(string userId, int organizationId)
        {
            var staff = new User();
            var organization = new Organization();
            var role = new Role();
            try
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OrganizationId == organizationId && u.Id == userId);
                if (user != null)
                {
                    staff = new User
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.UserName,
                        Address = user.Address,
                        CreatedBy = user.CreatedBy,
                        CreatedDate = user.CreatedDate,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        OrganizationId = user.OrganizationId,
                        PhoneNumber = user.PhoneNumber,
                        RoleId = user.RoleId,
                        UserName = user.UserName,
                        UserCode = user.UserCode,
                        Status = user.Status,
                        EmploymentDate = user.EmploymentDate,
                        Country = user.Country,
                        State = user.State,
                        ProfilePicture = user.ProfilePicture
                    };
                    organization = await _context.Organizations.FirstOrDefaultAsync(u => u.Id == user.OrganizationId);
                    role = await _context.Roles.FirstOrDefaultAsync(u => u.Id == user.RoleId);
                }

                return (Result.Success(), staff, organization, role);
            }
            catch (Exception ex)
            {
                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staff, organization, role);
            }
        }

        public async Task<(Result result, List<User> staffs)> GetUsersByOrganizationId(int organizationId)
        {
            var staffs = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Where(u => u.OrganizationId == organizationId).ToListAsync();

                foreach (var appUser in appUsers)
                {
                    User staff = new User();
                    staff.UserId = appUser.UserId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.JobFunctionId = appUser.JobFunctionId.HasValue ? appUser.JobFunctionId.Value : 0;
                    staff.UserCode = appUser.UserCode;
                    staff.Status = appUser.Status;
                    staff.UserName = appUser.UserName;
                    staff.LastModifiedBy = appUser.LastModifiedBy;
                    staff.LastModifiedDate = appUser.LastModifiedDate;
                    staff.EmploymentDate = appUser.EmploymentDate;
                    staff.Country = appUser.Country;
                    staff.State = appUser.State;
                    staff.JobFunctionId = appUser.JobFunctionId.HasValue ? appUser.JobFunctionId.Value : 0;
                    staff.ProfilePicture = appUser.ProfilePicture;
                    staffs.Add(staff);
                }
                return (Result.Success(), staffs);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staffs);
            }
        }
        public async Task<(Result Result, User staff)> GetUserByEmail(string email)
        {
            try
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (appUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with email: {email}" }), null);
                }
                var staff = new User
                {
                    UserId = appUser.UserId,
                    Address = appUser.Address,
                    CreatedBy = appUser.CreatedBy,
                    CreatedDate = appUser.CreatedDate,
                    DateOfBirth = appUser.DateOfBirth,
                    Email = appUser.Email,
                    FirstName = appUser.FirstName,
                    Gender = appUser.Gender,
                    LastAccessedDate = appUser.LastAccessedDate,
                    LastName = appUser.LastName,
                    Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ",
                    OrganizationId = appUser.OrganizationId,
                    PhoneNumber = appUser.PhoneNumber,
                    RoleId = appUser.RoleId,
                    JobFunctionId = appUser.JobFunctionId.HasValue? appUser.JobFunctionId.Value:0,
                    UserCode = appUser.UserCode,
                    Status = appUser.Status,
                    UserName = appUser.UserName,
                    LastModifiedBy = appUser.LastModifiedBy,
                    LastModifiedDate = appUser.LastModifiedDate,
                    EmploymentDate = appUser.EmploymentDate,
                    Country = appUser.Country,
                    State = appUser.State,
                };

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving Customer by username", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result result, List<User> staffs)> GetUsersByDepartmentId(int departmentId)
        {
            var staffs = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Where(u => u.DepartmentId == departmentId).ToListAsync();

                foreach (var appUser in appUsers)
                {
                    User staff = new User();
                    staff.UserId = appUser.UserId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.UserCode = appUser.UserCode;
                    staff.Status = appUser.Status;
                    staff.UserName = appUser.UserName;
                    staff.LastModifiedBy = appUser.LastModifiedBy;
                    staff.LastModifiedDate = appUser.LastModifiedDate;
                    staff.EmploymentDate = appUser.EmploymentDate;
                    staff.Country = appUser.Country;
                    staff.State = appUser.State;
                    staff.ProfilePicture = appUser.ProfilePicture;
                    staffs.Add(staff);
                }
                return (Result.Success(), staffs);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staffs);
            }
        }

        public async Task<(Result Result, User staff)> GetUserByIdOrganizationAndPermissions(string userId, int organizationId, int accessLevelId)
        {
            var staff = new User();
            try
            {
                var userd = await GetUserByIdAndOrganization(userId, organizationId);

                //////
                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staff);
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


        public async Task<(Result Result, List<User> users)> GetUsersByJobFunctionAndOrganization(int? jobfunctonId, int? organizationId)
        {
            var staffs = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Where(u => u.OrganizationId == organizationId && u.JobFunctionId == jobfunctonId).ToListAsync();

                foreach (var appUser in appUsers)
                {
                    User staff = new User();
                    staff.UserId = appUser.UserId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.JobFunctionId = appUser.JobFunctionId.HasValue ? appUser.JobFunctionId.Value : 0;
                    staff.UserCode = appUser.UserCode;
                    staff.Status = appUser.Status;
                    staff.UserName = appUser.UserName;
                    staff.LastModifiedBy = appUser.LastModifiedBy;
                    staff.LastModifiedDate = appUser.LastModifiedDate;
                    staff.EmploymentDate = appUser.EmploymentDate;
                    staff.Country = appUser.Country;
                    staff.State = appUser.State;
                    staff.ProfilePicture = appUser.ProfilePicture;
                    staffs.Add(staff);
                }
                return (Result.Success(), staffs);
            }

            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving user", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

    }
}

