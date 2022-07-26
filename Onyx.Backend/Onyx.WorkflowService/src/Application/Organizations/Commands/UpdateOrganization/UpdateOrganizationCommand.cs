using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization
{
    public partial class UpdateOrganizationCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RCNumber { get; set; }

        public string Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhoneNumber { get; set; }

        public Status Status { get; set; }
        public string OrganizationLogo { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public UpdateOrganizationCommandHandler(IApplicationDbContext context ,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
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
                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Name == request.Name && a.Id == request.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Organization details does not exist" });
                }
              

                org.Name = request.Name.Trim();
                org.RCNumber = request.RCNumber.Trim();
                org.LastModifiedDate = DateTime.Now;
                org.Address = request.Address.Trim();
                org.ContactEmail = request.ContactEmail.Trim();
                org.ContactPhoneNumber = request.ContactPhoneNumber.Trim();
                org.LogoFileLocation = request.OrganizationLogo;
                org.Country = request.Country;
                org.State = request.State;
                // Update file location
                //var bytes = Convert.FromBase64String(request.OrganizationLogo);
                ////create path for storing file
                //var uploadDirectory = Path.Combine("images\\");
                //if (!Directory.Exists(uploadDirectory))
                //{
                //    Directory.CreateDirectory(uploadDirectory);
                //}
                //string file = Path.Combine(uploadDirectory, request.RCNumber + ".jpg");
                //if (bytes.Length > 0)
                //{
                //    using (var stream = new FileStream(file, FileMode.Create))
                //    {
                //        org.LogoFileLocation = bytes;
                //        stream.Write(bytes, 0, bytes.Length);
                //        stream.Flush();
                //    }
                //}
                org.LastModifiedBy = user.staff.UserName;
                org.Status = request.Status;
                _context.Organizations.Update(org);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Organization updated successfully", org);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error updating organization : ", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
