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

namespace Onyx.AuthService.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommand :  IRequest<Result>
    {
        public string Name { get; set; }
        public string RCNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string LogoFileLocation { get; set; }
        public string ThemeColor { get; set; }
        public string UserId { get; set; }
    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;

        public CreateOrganizationCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter fileConverter,IEmailService emailService)
        {
            _context = context;
            _identityService = identityService;
            _fileConverter = fileConverter;
            _emailService = emailService;
        }

        public async Task<Result> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
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
                    Code = orgCode.Trim(),
                    ContactEmail = request.ContactEmail.Trim(),
                    ContactPhoneNumber = request.ContactPhoneNumber.Trim(),
                    Country = request.Country.Trim(),
                    State = request.State.Trim(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    RCNumber = request.RCNumber.Trim(),
                    Status = Status.Active,
                    LogoFileLocation = await _fileConverter.ConvertBase64StringToFile(request.LogoFileLocation , orgCode.Trim() + ".png"),
                   
                };

                var email = new EmailVm
                {
                    Application = "Onyx",
                    Subject = "Create Organization",
                    Text = "Organization has been created successfully",
                    RecipientEmail = request.ContactEmail,
                    RecipientName = request.Name,
                    ButtonText = "Verify Your Email",
                    Body = ""
                };
                await _emailService.OrganizationSignUp(email);

                _context.Organizations.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Organization created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Organization creation was not successful", ex?.Message??ex?.InnerException.Message });
            }
        }
    }
}
