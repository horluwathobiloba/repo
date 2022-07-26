using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Onyx.AuthService.Application.Organizations.Commands.UpdateOrganization
{
    public partial class UpdateOrganizationCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string RCNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public Status Status { get; set; }
        public string LogoFileLocation { get; set; }
        public string ThemeColor { get; set; }
        public string UserId { get; set; }

    }

    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;
        public UpdateOrganizationCommandHandler(IApplicationDbContext context ,IIdentityService identityService, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check userid and organization
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to update Organization.Invalid User ID and Organization credentials!" });
                }
                
                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Id == request.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Organization details does not exist" });
                }
              

                org.Name = request.Name.Trim();
                org.RCNumber = request.RCNumber.Trim();
                org.LastModifiedDate = DateTime.Now;
                org.Address = request.Address.Trim();
                org.Country = request.Country.Trim();
                org.State = request.State.Trim();
                org.ContactEmail = request.ContactEmail.Trim();
                org.ContactPhoneNumber = request.ContactPhoneNumber.Trim();
                if (!request.LogoFileLocation.Contains("https://"))
                {
                    org.LogoFileLocation = await _fileConverter.ConvertBase64StringToFile(request.LogoFileLocation, DateTime.Now.Ticks + "_"+ org.Code.Trim() + ".png");
                }
                org.LastModifiedBy = user.staff.UserName;
                org.Status = request.Status;
                org.StatusDesc = request.Status.ToString();
                _context.Organizations.Update(org);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Organization updated successfully", org);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error updating organization : ", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
