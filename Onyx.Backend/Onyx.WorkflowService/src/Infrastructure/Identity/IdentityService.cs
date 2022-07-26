using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Onyx.WorkFlowService.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<(Result Result, string UserId)> CreateStaffAsync(Staff staff)
        {
            try
            {
                var user = new ApplicationUser
                {
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.UserName,
                    Address = staff.Address,
                    CreatedBy = staff.CreatedBy,
                    CreatedDate = staff.CreatedDate,
                    DateOfBirth = staff.DateOfBirth,
                    SupervisorId = staff.SupervisorId,
                    DepartmentId = staff.DepartmentId,
                    Gender = staff.Gender,
                    MarketId = staff.MarketId,
                    MiddleName = staff.MiddleName,
                    OrganizationId = staff.OrganizationId,
                    PhoneNumber = staff.PhoneNumber,
                    RoleId = staff.RoleId,
                    UserName = staff.UserName,
                    StaffCode = staff.StaffCode,
                    Status = Domain.Enums.Status.Inactive,
                    EmploymentDate = staff.EmploymentDate,
                    Country = staff.Country,
                    State = staff.State
                   
                };
                var result = await _userManager.CreateAsync(user, staff.Password);
                //update Staff Id
                user.StaffId = user.Id;
                await _userManager.UpdateAsync(user);
                return (result.ToApplicationResult(), user.Id);
            }
            catch (Exception ex)
            {
              
                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), "");
            }
        }

        public async Task<Result> ChangeStaffStatusAsync(Staff staff)
        {
            try
            {
               
                var appUser = await _userManager.Users.FirstOrDefaultAsync(a=>a.Id == staff.StaffId);
                string message = "";
                if (appUser != null)
                {
                    switch (appUser.Status)
                    {
                        case Domain.Enums.Status.Active:
                            appUser.Status = Domain.Enums.Status.Inactive;
                           message = "Staff deactivation was successful";
                            break;
                        case Domain.Enums.Status.Inactive:
                            appUser.Status = Domain.Enums.Status.Active;
                            message = "Staff activation was successful";
                            break;
                        case Domain.Enums.Status.Deactivated:
                            appUser.Status = Domain.Enums.Status.Active;
                           message = "Staff activation was successful";
                            break;
                        default:
                            break;
                    }
                };
                    return await ChangeStaffStatusAsync(appUser, message);
               
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error changing user status", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<Result> ChangeStaffStatusAsync(ApplicationUser user,string message)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Result.Success(message);
                else
                    return Result.Failure( result.ToApplicationResult().Messages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Result> UpdateStaffAsync(Staff staff)
        {
            try
            {
                var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == staff.StaffId);
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
                userToUpdate.SupervisorId = staff.SupervisorId;
                userToUpdate.DepartmentId = staff.DepartmentId;
                userToUpdate.Gender = staff.Gender;
                userToUpdate.MarketId = staff.MarketId;
                userToUpdate.MiddleName = staff.MiddleName;
                userToUpdate.OrganizationId = staff.OrganizationId;
                userToUpdate.PhoneNumber = staff.PhoneNumber;
                userToUpdate.RoleId = staff.RoleId;
                userToUpdate.UserName = staff.UserName;
                userToUpdate.StaffCode = staff.StaffCode;
                userToUpdate.Status = staff.Status;
                userToUpdate.EmploymentDate = staff.EmploymentDate;
                userToUpdate.Country = staff.Country;
                userToUpdate.State = staff.State;
                return Result.Success("User was updated successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating user", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<(Result result, List<Staff> staffs)> GetAll()
        {

            List<Staff> staffs = new List<Staff>();
            try
            {
                var appUsers = await _userManager.Users.Include(a => a.Role).ToListAsync();
                if (appUsers == null)
                {
                    return (Result.Failure(new string[] { "No users exist on the system" }), null);
                }
                
                foreach (var appUser in appUsers)
                {
                    Staff staff = new Staff();
                    staff.StaffId = appUser.StaffId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.SupervisorId = appUser.SupervisorId;
                    staff.DepartmentId = appUser.DepartmentId;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.MarketId = appUser.MarketId;
                    staff.MiddleName = appUser.MiddleName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.Status = appUser.Status;
                    staff.StaffCode = appUser.StaffCode;
                    staff.SupervisorId = appUser.SupervisorId;
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

        public async Task<(Result Result, Staff staff)> GetUserById(string userId)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return (Result.Failure(new string[] { $"Staff does not exist with Id: {userId}" }), null);
                }
                var staff = new Staff
                {
                    StaffId = user.StaffId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.UserName,
                    Address = user.Address,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    DateOfBirth = user.DateOfBirth,
                    DepartmentId = user.DepartmentId,
                    SupervisorId = user.SupervisorId,
                    Gender = user.Gender,
                    MarketId = user.MarketId,
                    MiddleName = user.MiddleName,
                    OrganizationId = user.OrganizationId,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    UserName = user.UserName,
                    StaffCode = user.StaffCode,
                    Status = user.Status,
                    EmploymentDate = user.EmploymentDate,
                    Country = user.Country,
                    State = user.State,
                };

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving staff", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<(Result Result, Staff staff)> GetUserByUsername(string userName)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with username: {userName}" }), null);
                }
                var staff = new Staff
                {
                    StaffId = user.StaffId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.UserName,
                    Address = user.Address,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    DateOfBirth = user.DateOfBirth,
                    DepartmentId = user.DepartmentId,
                    SupervisorId = user.SupervisorId,
                    Gender = user.Gender,
                    MarketId = user.MarketId,
                    MiddleName = user.MiddleName,
                    OrganizationId = user.OrganizationId,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    UserName = user.UserName,
                    StaffCode = user.StaffCode,
                    Status = user.Status,
                    EmploymentDate = user.EmploymentDate,
                    Country = user.Country,
                    State = user.State
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

        public async Task<(Result Result, Staff staff)> GetUserByIdAndOrganization(string userId, int organizationId)
        {
            var staff = new Staff();
            try
            {
                
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OrganizationId == organizationId && u.Id == userId);
                if (user!=null)
                {
                     staff = new Staff
                    {
                        StaffId = user.StaffId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.UserName,
                        Address = user.Address,
                        CreatedBy = user.CreatedBy,
                        CreatedDate = user.CreatedDate,
                        DateOfBirth = user.DateOfBirth,
                        SupervisorId = user.SupervisorId,
                        DepartmentId = user.DepartmentId,
                        Gender = user.Gender,
                        MarketId = user.MarketId,
                        MiddleName = user.MiddleName,
                        OrganizationId = user.OrganizationId,
                        PhoneNumber = user.PhoneNumber,
                        RoleId = user.RoleId,
                        UserName = user.UserName,
                        StaffCode = user.StaffCode,
                        Status = user.Status,
                        EmploymentDate = user.EmploymentDate,
                        Country = user.Country,
                        State = user.State
                    };
                }

                return (Result.Success(), staff);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staff);
            }
        }

        public async Task<(Result result, List<Staff> staffs)> GetStaffsByOrganizationId(int organizationId)
        {
            var staffs = new List<Staff>();
            try
            {
                var appUsers = await _userManager.Users.Where(u => u.OrganizationId == organizationId).ToListAsync();

                foreach (var appUser in appUsers)
                {
                    Staff staff = new Staff();
                    staff.StaffId = appUser.StaffId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.SupervisorId = appUser.SupervisorId;
                    staff.DepartmentId = appUser.DepartmentId;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.MarketId = appUser.MarketId;
                    staff.MiddleName = appUser.MiddleName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.StaffCode = appUser.StaffCode;
                    staff.SupervisorId = appUser.SupervisorId;
                    staff.Status = appUser.Status;
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

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staffs);
            }
        }

        public async Task<(Result result, List<Staff> staffs)> GetStaffsByDepartmentId(int departmentId)
        {
            var staffs = new List<Staff>();
            try
            {
                var appUsers = await _userManager.Users.Where(u => u.DepartmentId == departmentId).ToListAsync();

                foreach (var appUser in appUsers)
                {
                    Staff staff = new Staff();
                    staff.StaffId = appUser.StaffId;
                    staff.Address = appUser.Address;
                    staff.CreatedBy = appUser.CreatedBy;
                    staff.CreatedDate = appUser.CreatedDate;
                    staff.DateOfBirth = appUser.DateOfBirth;
                    staff.SupervisorId = appUser.SupervisorId;
                    staff.DepartmentId = appUser.DepartmentId;
                    staff.Email = appUser.Email;
                    staff.FirstName = appUser.FirstName;
                    staff.Gender = appUser.Gender;
                    staff.LastAccessedDate = appUser.LastAccessedDate;
                    staff.LastName = appUser.LastName;
                    staff.MarketId = appUser.MarketId;
                    staff.MiddleName = appUser.MiddleName;
                    staff.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    staff.OrganizationId = appUser.OrganizationId;
                    staff.PhoneNumber = appUser.PhoneNumber;
                    staff.RoleId = appUser.RoleId;
                    staff.StaffCode = appUser.StaffCode;
                    staff.SupervisorId = appUser.SupervisorId;
                    staff.Status = appUser.Status;
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

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), staffs);
            }
        }
    }
}
