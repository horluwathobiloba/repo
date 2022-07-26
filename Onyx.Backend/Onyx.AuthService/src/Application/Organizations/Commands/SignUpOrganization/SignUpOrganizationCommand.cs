using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Enums;
using System;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Onyx.AuthService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Onyx.AuthService.Application.Organizations.Commands.SignUpOrganization
{
    public class SignUpOrganizationCommand :  IRequest<Result>
    {
        public string Name { get; set; }
        public string RCNumber { get; set; }
        public string Address { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string LogoFileLocation { get; set; }
        public string ThemeColor { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public UserVm User { get; set; }

    }

    public class SignUpOrganizationCommandHandler : IRequestHandler<SignUpOrganizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ISqlService _sqlService;
        private readonly IStringHashingService _stringHashingService;

        public SignUpOrganizationCommandHandler(IApplicationDbContext context, IIdentityService identityService, 
            IBase64ToFileConverter fileConverter, IEmailService emailService, IConfiguration  configuration,
            ISqlService sqlService, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _fileConverter = fileConverter;
            _emailService = emailService;
            _configuration = configuration;
            _sqlService = sqlService;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(SignUpOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orgExists = await _context.Organizations.AnyAsync(a=>a.Name == request.Name 
                || a.RCNumber == request.RCNumber ||
                (a.Code == request.Name.Split(" ", StringSplitOptions.None)[0].ToUpper()
                && (a.ContactEmail == request.ContactEmail) 
                && (a.ContactPhoneNumber == request.ContactPhoneNumber)));
                if (orgExists)
                {
                    return Result.Failure(new string[] { "Organization details already exist" });
                }
                var orgCode = request.Name.Split(" ", StringSplitOptions.None)[0].ToUpper();
                var existingOrgCode = await _context.Organizations.Where(a=>a.Code == request.Name.Split(" ", StringSplitOptions.None)[0].ToUpper()
                                                                   && (a.ContactEmail != request.ContactEmail || a.ContactPhoneNumber != request.ContactPhoneNumber))
                                                                  .ToListAsync();
                if (existingOrgCode != null && existingOrgCode.Count > 0)
                {
                    orgCode = request.Name.Split(" ", StringSplitOptions.None)[0].ToUpper() + (existingOrgCode.Count+1);
                }
                var bytes = Convert.FromBase64String(request.LogoFileLocation);
                if (bytes.Length == 0)
                {
                    return Result.Failure(new string[] { "Invalid Logo details" });
                }

                var entity = new Organization
                {
                    Name = request.Name.Trim(),
                    Address = request.Address.Trim(),
                    State = request.State,
                    Country = request.Country,
                    Code = orgCode.Trim(),
                    ContactEmail = request.ContactEmail.Trim(),
                    ContactPhoneNumber = request.ContactPhoneNumber.Trim(),
                    CreatedBy = "",
                    CreatedById = "",
                    CreatedDate = DateTime.Now,
                    RCNumber = request.RCNumber.Trim(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    LogoFileLocation = await _fileConverter.ConvertBase64StringToFile(request.LogoFileLocation , orgCode.Trim() + ".png"),
                   
                };

                await _context.BeginTransactionAsync();
                _context.Organizations.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                var orgId = entity.Id;
                List<Role> roles = new List<Role>();
                var adminRole = new Role
                {
                    Name = "Admin",
                    CreatedDate = DateTime.Now,
                    AccessLevel = AccessLevel.Admin,
                    OrganizationId = orgId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedBy = request.User.Email,
                    CreatedById = request.User.Email,
                    WorkflowUserCategory = WorkflowUserCategory.Internal
                };
                roles.Add(adminRole);
                //create vendor role
                var vendorRole = new Role
                {
                    Name = "Vendor",
                    AccessLevel = AccessLevel.ExternalUser,
                    CreatedDate = DateTime.Now,
                    OrganizationId = orgId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedBy = request.User.Email,
                    CreatedById = request.User.Email,
                    WorkflowUserCategory = WorkflowUserCategory.External
                };
                roles.Add(vendorRole);
                //create Third Party role
                var thirdPartyRole = new Role
                {
                    Name = "Third Party",
                    AccessLevel = AccessLevel.ExternalUser,
                    CreatedDate = DateTime.Now,
                    OrganizationId = orgId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedBy = request.User.Email,
                    CreatedById = request.User.Email,
                    WorkflowUserCategory = WorkflowUserCategory.External
                };
                roles.Add(thirdPartyRole);
                //create Witness role
                var witnessRole = new Role
                {
                    Name = "Witness",
                    AccessLevel = AccessLevel.ExternalUser,
                    CreatedDate = DateTime.Now,
                    OrganizationId = orgId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedBy = request.User.Email,
                    CreatedById = request.User.Email,
                    WorkflowUserCategory = WorkflowUserCategory.External
                };
                roles.Add(witnessRole);
                //add all created roles
                _context.Roles.AddRange(roles);
                await _context.SaveChangesAsync(cancellationToken);

                //add permissions to creation of admin
                var commaSeperatedPermissions = new List<string>();
                var permissionsList = Enum.GetNames(typeof(AdminPermissions)).ToList();
                foreach (var permission in permissionsList)
                {
                    commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                }
                var rolePermissionsList = new List<RolePermissionsDT>();
                if (adminRole.Id != 0)
                {
                    foreach (var permission in commaSeperatedPermissions)
                    {
                        var rolePermission = new RolePermissionsDT
                        {

                            OrganizationId = orgId,
                            AccessLevel = (int)AccessLevel.Admin,
                            Permission = permission,
                            RoleId = adminRole.Id,
                            CreatedBy = request.User.Email,
                            CreatedById = request.User.Email,
                            CreatedDate = DateTime.Now,
                            Status = (int)Status.Active
                        };
                        rolePermissionsList.Add(rolePermission);
                    }
                }

                
                await _context.SaveChangesAsync(cancellationToken);
                await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                
                var user = new User
                {
                    Address = request.Address.Trim(),
                    State = request.State,
                    Country = request.Country,
                    PhoneNumber = request.User.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    //DateOfBirth = request.User.DateOfBirth,
                    Email = request.User.Email,
                   // EmploymentDate = request.User.EmploymentDate,
                    FirstName = request.User.FirstName,
                    LastAccessedDate = DateTime.Now,
                    //Gender = request.User.Gender,
                    LastName = request.User.LastName,
                    Name = string.Concat(request.User.FirstName, " ", request.User.LastName),
                    OrganizationId = orgId,
                    UserName = request.User.Email,
                    Password = request.User.Password,
                    UserCode = "00" + 1,
                    RoleId = adminRole.Id,
                    
                };
                var hashValue = (request.User.Email + DateTime.Now).ToString();
                user.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                var userCreationResponse = await _identityService.CreateUserAsync(user);
                if (!userCreationResponse.Result.Succeeded)
                {
                    _context.RollbackTransaction();
                    return (Result.Failure(userCreationResponse.Result.Messages.FirstOrDefault()));
                }
                await _context.SaveChangesAsync(cancellationToken);
                var signUpItems = new
                {
                    Organization = entity,
                    User = user,
                    Role = adminRole
                };
              
                var email = new EmailVm
                {
                    Application = "Onyx",
                    Subject = "Organization Creation",
                    Text = "Organization has been created successfully",
                    RecipientEmail = request.ContactEmail,
                    RecipientName = request.Name,
                    ButtonText = "Organization Creation was successful",
                    Body = ""
                };

                string webDomain = _configuration["WebDomain"];
                var admin_email = new EmailVm
                {
                    Application = "Onyx",
                    Subject = "Admin Email Setup",
                    BCC = "",
                    CC = "",
                    RecipientEmail = request.User.Email,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Password = request.User.Password,
                    OrganizationCode = orgCode,
                    Body = "Your Account has been created successfully!",
                    Body1 = "Click the button below to verify your account.",
                    ButtonText = "Verify Your Account",
                    ButtonLink = webDomain + $"login?email={request.User.Email}&token={user.Token}'",
                    ImageSource = _configuration["SVG:EmailVerification"],


                    //Application = "Onyx",
                    //Subject = "Admin Email Setup",
                    //Text = "Your credentials  has been created successfully",
                    //RecipientEmail = request.User.Email,
                    //FirstName = request.User.FirstName,
                    //LastName = request.User.LastName,
                    //Password = request.User.Password,
                    //Body1 = $"Thank you for signing up {request.User.FirstName}",
                    //Body2 = "Organization Code: " + orgCode,
                    //ButtonText = $"<a href='{webDomain}login?email={request.User.Email}'>Verify your account</a>",
                    //ImageSource = _configuration["SVG:EmailVerification"],
                    //// DisplayButton = "display:none;"
                    //DisplayButton = ""
                };
                await _emailService.OrganizationSignUp(email);
                await _emailService.AdminEmailVerification(admin_email);
                await _context.CommitTransactionAsync();

                return Result.Success("Organization sign up successful", signUpItems);
            }
            catch (Exception ex)
            {

                 _context.RollbackTransaction();
                return Result.Failure(new string[] { "Organization sign up was not successful", ex?.Message??ex?.InnerException.Message });
            }
        }
    }
}


