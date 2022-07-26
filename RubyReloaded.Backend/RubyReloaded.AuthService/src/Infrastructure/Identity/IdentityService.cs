using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Domain.Entities;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;

namespace RubyReloaded.AuthService.Infrastructure.Identity
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

                var newUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Status = Domain.Enums.Status.Inactive,
                    Country = user.Country,
                    //RoleId=user.RoleId,
                    State = user.State,
                    EmailConfirmed = true,
                    ProfilePicture = user.ProfilePicture,
                };
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (!result.Succeeded)
                {
                    return (Result.Failure(new string[] { result.Errors.FirstOrDefault().Description }), "");
                }
                //update User Id
                newUser.UserId = newUser.Id;

                await _userManager.UpdateAsync(newUser);
                //TODO :Add Customer operation afetr evry user operation
                var customer = user as Customer;
                var customerObject = new Customer
                {
                    Address = user.Address,
                    State = user.State,
                    NotificationStatus = user.NotificationStatus,
                    BVN = user.BVN,
                    Country = user.Country,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedDate = user.CreatedDate,
                    Name = user.Name,
                    Password = user.Password,
                    Status = user.Status,
                    StatusDesc = user.StatusDesc,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    TransactionPin = user.TransactionPin,
                    UserId = user.UserId,
                    UserAccessLevel = user.UserAccessLevel,
                    UserName = user.UserName,
                };
                await _context.Customers.AddAsync(customerObject);
                await _context.SaveChangesAsync(new System.Threading.CancellationToken());
                //var customer = (Customer)user;

                return (result.ToApplicationResult(), newUser.Id);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), "");
            }
        }

        //public async Task<Result> ChangeUserStatusAsync(User user)
        public async Task<Result> ChangeUserStatusAsync(Customer user)
        {
            try
            {
                //for customer
                //var appUser = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == user.UserId);
                //var appUser = await _context.Customers.FirstOrDefaultAsync(a => a.UserId == user.UserId);
                string message = "";
                //if (appUser != null)
                //{
                //    switch (appUser.Status)
                //    {
                //        case Domain.Enums.Status.Active:
                //            appUser.Status = Domain.Enums.Status.Inactive;
                //            message = "User deactivation was successful";
                //            break;
                //        case Domain.Enums.Status.Inactive:
                //            appUser.Status = Domain.Enums.Status.Active;
                //            message = "User activation was successful";
                //            break;
                //        case Domain.Enums.Status.Deactivated:
                //            appUser.Status = Domain.Enums.Status.Active;
                //            message = "User activation was successful";
                //            break;
                //        default:
                //            break;
                //    }
                //};

                var appCustomer = await _context.Customers.FirstOrDefaultAsync(a => a.UserId == user.UserId);
                if (appCustomer != null)
                {
                    switch (appCustomer.Status)
                    {
                        case Domain.Enums.Status.Active:
                            appCustomer.Status = Domain.Enums.Status.Inactive;
                            message = "Customer deactivation was successful";
                            break;
                        case Domain.Enums.Status.Inactive:
                            appCustomer.Status = Domain.Enums.Status.Active;
                            message = "Customer activation was successful";
                            break;
                        case Domain.Enums.Status.Deactivated:
                            appCustomer.Status = Domain.Enums.Status.Active;
                            message = "Customer activation was successful";
                            break;
                        default:
                            break;
                    }
                };


                _context.Customers.Update(appCustomer);
                await _context.SaveChangesAsync(new System.Threading.CancellationToken());
                return Result.Success(message);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error changing user status", ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        /*public async Task<Result> ChangeUserStatusAsync(ApplicationUser user, string message)*/

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



        public async Task<Result> ChangeUserStatusAsync(User user)
        {
            try
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == user.UserId);
                var appCustomer = await _context.Customers.FirstOrDefaultAsync(a => a.Id == user.Id);
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
                if (appCustomer != null)
                {
                    switch (appCustomer.Status)
                    {
                        case Domain.Enums.Status.Active:
                            appCustomer.Status = Domain.Enums.Status.Inactive;

                            break;
                        case Domain.Enums.Status.Inactive:
                            appCustomer.Status = Domain.Enums.Status.Active;

                            break;
                        case Domain.Enums.Status.Deactivated:
                            appCustomer.Status = Domain.Enums.Status.Active;

                            break;
                        default:
                            break;
                    }
                };
                _context.Customers.Update(appCustomer);
                await _context.SaveChangesAsync(new System.Threading.CancellationToken());
                return await ChangeUserStatusAsync(appUser, message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error changing user status", ex?.Message ?? ex?.InnerException?.Message });
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
                userToUpdate.Address = user.Address;
                userToUpdate.CreatedBy = user.CreatedBy;
                userToUpdate.CreatedDate = user.CreatedDate;
                userToUpdate.DateOfBirth = user.DateOfBirth;
                userToUpdate.PhoneNumber = user.PhoneNumber;
                userToUpdate.Status = user.Status;
                userToUpdate.Country = user.Country;
                userToUpdate.State = user.State;
                userToUpdate.ProfilePicture = user.ProfilePicture;
                await _userManager.UpdateAsync(userToUpdate);
                // For Customer
                var customerToUpdate = await _context.Customers.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                if (customerToUpdate == null) return Result.Failure(new string[] { "Customer for update does not exist" });

                customerToUpdate.FirstName = user.FirstName;
                customerToUpdate.LastName = user.LastName;
                customerToUpdate.Email = user.Email;
                customerToUpdate.Address = user.Address;
                customerToUpdate.CreatedBy = user.CreatedBy;
                customerToUpdate.CreatedDate = user.CreatedDate;
                customerToUpdate.DateOfBirth = user.DateOfBirth;
                customerToUpdate.PhoneNumber = user.PhoneNumber;
                customerToUpdate.Status = user.Status;
                customerToUpdate.Country = user.Country;
                customerToUpdate.State = user.State;
                customerToUpdate.ProfilePicture = user.ProfilePicture;
                _context.Customers.Update(customerToUpdate);
                await _context.SaveChangesAsync(new System.Threading.CancellationToken());
                return Result.Success("User was updated successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating user", ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        public async Task<(Result result, List<User> users)> GetAll(int skip, int take)
        {
            List<User> users = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Skip(skip)
                                          .Take(take)
                                          .ToListAsync();
                if (appUsers == null)
                {
                    return (Result.Failure(new string[] { "No users exist on the system" }), null);
                }

                foreach (var appUser in appUsers)
                {
                    User user = new User();
                    user.UserId = appUser.UserId;
                    user.Address = appUser.Address;
                    user.CreatedBy = appUser.CreatedBy;
                    user.CreatedDate = appUser.CreatedDate;
                    user.DateOfBirth = appUser.DateOfBirth;
                    user.Email = appUser.Email;
                    user.FirstName = appUser.FirstName;
                    user.LastAccessedDate = appUser.LastAccessedDate;
                    user.LastName = appUser.LastName;
                    user.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.Status = appUser.Status;
                    user.LastModifiedBy = appUser.LastModifiedBy;
                    user.LastModifiedDate = appUser.LastModifiedDate;
                    user.Country = appUser.Country;
                    user.State = appUser.State;
                    users.Add(user);
                }

                // For customers
                var appCustomers = await _context.Customers.Skip(skip).Take(take).ToListAsync();
                if (appCustomers == null)
                {
                    return (Result.Failure(new string[] { "No customers exist on the system" }), null);
                }

                foreach (var appCustomer in appCustomers)
                {
                    Customer customer = new Customer();
                    customer.UserId = appCustomer.UserId;
                    customer.Address = appCustomer.Address;
                    customer.CreatedBy = appCustomer.CreatedBy;
                    customer.CreatedDate = appCustomer.CreatedDate;
                    customer.DateOfBirth = appCustomer.DateOfBirth;
                    customer.Email = appCustomer.Email;
                    customer.FirstName = appCustomer.FirstName;
                    customer.LastAccessedDate = appCustomer.LastAccessedDate;
                    customer.LastName = appCustomer.LastName;
                    customer.Name = appCustomer.FirstName + " " + appCustomer?.LastName ?? " ";
                    customer.PhoneNumber = appCustomer.PhoneNumber;
                    customer.Status = appCustomer.Status;
                    customer.LastModifiedBy = appCustomer.LastModifiedBy;
                    customer.LastModifiedDate = appCustomer.LastModifiedDate;
                    customer.Country = appCustomer.Country;
                    customer.State = appCustomer.State;
                }

                return (Result.Success(), users);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving users", ex?.Message ?? ex?.InnerException?.Message }), users);
            }
        }




        public async Task<(Result result, List<User> users)> GetAllSystemOwners(int skip, int take)
        {
            List<User> users = new List<User>();
            try
            {
                var appUsers = await _userManager.Users.Where(a => a.UserAccessLevel == Domain.Enums.UserAccessLevel.SystemOwner).Skip(skip)
                                          .Take(take)
                                          .ToListAsync();
                if (appUsers == null)
                {
                    return (Result.Failure(new string[] { "No users exist on the system" }), null);
                }

                foreach (var appUser in appUsers)
                {
                    User user = new User();
                    user.UserId = appUser.UserId;
                    user.Address = appUser.Address;
                    user.CreatedBy = appUser.CreatedBy;
                    user.CreatedDate = appUser.CreatedDate;
                    user.DateOfBirth = appUser.DateOfBirth;
                    user.Email = appUser.Email;
                    user.FirstName = appUser.FirstName;
                    user.LastAccessedDate = appUser.LastAccessedDate;
                    user.LastName = appUser.LastName;
                    user.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.Status = appUser.Status;
                    user.LastModifiedBy = appUser.LastModifiedBy;
                    user.LastModifiedDate = appUser.LastModifiedDate;
                    user.Country = appUser.Country;
                    user.State = appUser.State;
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
                var existingUser = await _userManager.FindByIdAsync(userId);
                if (existingUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with Id: {userId}" }), null);
                }
                var user = new User
                {
                    UserId = existingUser.UserId,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email,
                    Address = existingUser.Address,
                    CreatedBy = existingUser.CreatedBy,
                    CreatedDate = existingUser.CreatedDate,
                    DateOfBirth = existingUser.DateOfBirth,
                    PhoneNumber = existingUser.PhoneNumber,
                    Status = existingUser.Status,
                    Country = existingUser.Country,
                    State = existingUser.State,
                    ProfilePicture = existingUser.ProfilePicture,
                    BVN = existingUser.BVN,
                    TransactionPin = existingUser.TransactionPin,
                    UserName = existingUser.UserName
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
                    UserId = existingUser.UserId,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email,
                    Address = existingUser.Address,
                    CreatedBy = existingUser.CreatedBy,
                    CreatedDate = existingUser.CreatedDate,
                    DateOfBirth = existingUser.DateOfBirth,
                    PhoneNumber = existingUser.PhoneNumber,
                    Status = existingUser.Status,
                    Country = existingUser.Country,
                    State = existingUser.State,
                    ProfilePicture = existingUser.ProfilePicture,
                    BVN = existingUser.BVN,
                    TransactionPin = existingUser.TransactionPin,
                    UserName = existingUser.UserName
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

        public async Task<(Result Result, User user)> GetUserByIdAndOrganization(string userId, int organizationId)
        {
            var user = new User();
            try
            {

                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.OrganizationId == organizationId && u.Id == userId);
                if (existingUser != null)
                {
                    user = new User
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        CreatedBy = user.CreatedBy,
                        CreatedDate = user.CreatedDate,
                        DateOfBirth = user.DateOfBirth,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Status = user.Status,
                        Country = user.Country,
                        State = user.State,
                        ProfilePicture = user.ProfilePicture
                    };
                }

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving username", ex?.Message ?? ex?.InnerException?.Message }), user);
            }
        }

        public async Task<(Result Result, User user)> GetUserByEmail(string email)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                if (appUser == null)
                {
                    return (Result.Failure(new string[] { $"User does not exist with email: {email}" }), null);
                }

                var user = new User
                {
                    UserId = appUser.UserId,
                    Address = appUser.Address,
                    CreatedBy = appUser.CreatedBy,
                    CreatedDate = appUser.CreatedDate,
                    DateOfBirth = appUser.DateOfBirth,
                    Email = appUser.Email,
                    FirstName = appUser.FirstName,
                    LastAccessedDate = appUser.LastAccessedDate,
                    LastName = appUser.LastName,
                    UserName = appUser.UserName,
                    Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ",
                    PhoneNumber = appUser.PhoneNumber,
                    Status = appUser.Status,
                    LastModifiedBy = appUser.LastModifiedBy,
                    LastModifiedDate = appUser.LastModifiedDate,
                    Country = appUser.Country,
                    State = appUser.State,
                    BVN = appUser.BVN,
                    TransactionPin = appUser.TransactionPin
                };

                return (Result.Success(), user);
            }
            catch (Exception ex)
            {

                return (Result.Failure(new string[] { "Error retrieving user by username", ex?.Message ?? ex?.InnerException?.Message }), null);
            }
        }

        public async Task<Result> VerifyEmailAsync(User user)
        {
            try
            {
                var verifyUser = await _userManager.FindByEmailAsync(user.Email);
                if (verifyUser == null)
                {
                    return Result.Failure("User does not exist");
                }

                if (verifyUser.Verified)
                {
                    return Result.Failure("Email is confirmed");
                }

                verifyUser.Status = Domain.Enums.Status.Active;

                verifyUser.Verified = true;
                await _userManager.UpdateAsync(verifyUser);
                return Result.Success("Email verified successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error verifying email", ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        public async Task<Result> ChangeUserPhoneNumber(string email, string phoneNumber)
        {
            try
            {
                var verifyUser = await _userManager.FindByEmailAsync(email);
                if (verifyUser == null)
                {
                    return Result.Failure("User does not exist");
                }
                await _userManager.SetPhoneNumberAsync(verifyUser, phoneNumber);
                return Result.Success("PhoneNumber updated successfully");
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
        public async Task<(bool success, string token, string reason)> GenerateOTP(string email, string reason)
        {
            //work must be done here
            var user = await _userManager.FindByEmailAsync(email);
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email"))
            {
                //Email of the user must be confirmed
                return (false, "", "");
            }
            if (user == null)
            {
                return (false, "", "");
            }
            var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            if (otp != null)
            {
                return (true, otp, reason);
            }
            return (false, "", "");
        }
        public async Task<(bool success, string message)> ValidateOTP(string otp, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "");
            }
            var val = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", otp);
            if (val)
            {
                return (true, "Verified");
            }
            return (false, "");
        }

        public async Task<(bool success, List<AjoCode> ajoCodes, string message)> GenerateAjoCodes(int ajoid, List<string> emails)
        {
            try
            {
                var ajocodes = new List<AjoCode>();
                foreach (var email in emails)
                {
                    Random rd = new Random();
                    int rand_num = rd.Next(1000, 9000);
                    var ajocode = new AjoCode
                    {
                        Code = rand_num.ToString(),
                        CreatedDate = DateTime.Now,
                        AjoId = ajoid,
                        IsUsed = false,
                        Status = Domain.Enums.Status.Active,
                        StatusDesc = Domain.Enums.Status.Active.ToString(),
                        Email = email,
                    };
                    ajocodes.Add(ajocode);
                }

                return (true, ajocodes, "AjoCode created Successfully");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<(bool success, CooperativeUserCode coopCode, string message)> GenerateCooperativeCode(int cooperativeid, string email)
        {
            try
            {
                Random rd = new Random();

                int rand_num = rd.Next(1000, 9000);
                var cooperativecode = new CooperativeUserCode
                {
                    Code = rand_num.ToString(),
                    CreatedDate = DateTime.Now,
                    CooperativeId = cooperativeid,
                    IsUsed = false,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),
                    Email = email,
                };
                return (true, cooperativecode, "AjoCode created Successfully");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message ?? ex.InnerException.Message);
            }
        }


        public async Task<(bool success, List<CooperativeUserCode> coopCodes, string message)> GenerateCooperativeCodes(int cooperativeid, List<string> emails)
        {
            try
            {
                var coopCodes = new List<CooperativeUserCode>();
                foreach (var email in emails)
                {
                    Random rd = new Random();

                    int rand_num = rd.Next(1000, 9000);
                    var cooperativecode = new CooperativeUserCode
                    {
                        Code = rand_num.ToString(),
                        CreatedDate = DateTime.Now,
                        CooperativeId = cooperativeid,
                        IsUsed = false,
                        Status = Domain.Enums.Status.Active,
                        StatusDesc = Domain.Enums.Status.Active.ToString(),
                        Email = email,
                    };
                    coopCodes.Add(cooperativecode);
                }

                return (true, coopCodes, "CooperativeUserCodes created Successfully");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<(bool success, AjoCode ajoCode, string message)> GenerateAjoCode(int ajoid, string email)
        {
            try
            {
                Random rd = new Random();
                int rand_num = rd.Next(1000, 9000);
                var ajocode = new AjoCode
                {
                    Code = rand_num.ToString(),
                    CreatedDate = DateTime.Now,
                    AjoId = ajoid,
                    IsUsed = false,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),
                    Email = email,
                };
                return (true, ajocode, "AjoCode created Successfully");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<Result> ChangeTransactionPin(string userId, string pin)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result.Failure("User cannot be found");
                }
                user.TransactionPin = pin;
                await _userManager.UpdateAsync(user);
                return Result.Success(user);
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<Result> UpdateBVN(string userId, string bvn)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result.Failure("User cannot be found");
                }
                user.BVN = bvn;
                await _userManager.UpdateAsync(user);
                return Result.Success(user);
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message ?? ex.InnerException.Message);
            }
        }
        public async Task<Result> VerifyTransactionPin(string userId, string pin)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return Result.Failure("User cannot be found");
                }
                if (user.TransactionPin != pin)
                {
                    return Result.Failure("Invalid User Transaction pin");
                }

                return Result.Success("Transaction Pin is Valid", new { valid = true });
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<Result> ChangeUserNotificationStatusAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Result.Failure("UserId is null");
                }
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                {
                    return Result.Failure("User cannot be found");
                }
                user.NotificationStatus = !user.NotificationStatus;
                await _userManager.UpdateAsync(user);
                return Result.Success(user);
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message ?? ex.InnerException.Message);
            }
        }

        public async Task<Result> GetUsersDashboardAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var total = users.Count();
                var active = users.Where(x => x.Status == Domain.Enums.Status.Active).Count();
                var nonactive = users.Where(x => x.Status == Domain.Enums.Status.Inactive).Count();

                return Result.Success(new { total, active, nonactive });
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message ?? ex.InnerException.Message);
            }
        }

        //public Task<Result> ChangeUserStatusAsync(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<(Result result, List<User> users)> GetAllSystemOwners(int skip, int take)
        //{
        //    try
        //    {
        //        List<User> users = new List<User>();
        //        var systemUsers = await _userManager.Users.Where(x => x.UserAccessLevel == Domain.Enums.UserAccessLevel.SystemOwner).Skip(skip)
        //                                      .Take(take)
        //                                      .ToListAsync();
        //        foreach (var appUser in systemUsers)
        //        {
        //            User user = new User();
        //            user.UserId = appUser.UserId;
        //            user.Address = appUser.Address;
        //            user.CreatedBy = appUser.CreatedBy;
        //            user.CreatedDate = appUser.CreatedDate;
        //            user.DateOfBirth = appUser.DateOfBirth;
        //            user.Email = appUser.Email;
        //            user.FirstName = appUser.FirstName;
        //            user.LastAccessedDate = appUser.LastAccessedDate;
        //            user.LastName = appUser.LastName;
        //            user.Name = appUser.FirstName + " " + appUser?.MiddleName ?? " " + " " + appUser?.LastName ?? " ";
        //            user.PhoneNumber = appUser.PhoneNumber;
        //            user.Status = appUser.Status;
        //            user.LastModifiedBy = appUser.LastModifiedBy;
        //            user.LastModifiedDate = appUser.LastModifiedDate;
        //            user.Country = appUser.Country;
        //            user.State = appUser.State;
        //            users.Add(user);
        //        }
        //        return (Result.Success(), users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return (Result.Failure(ex.Message ?? ex.InnerException.Message),null);
        //    }

        //}
    }
}

