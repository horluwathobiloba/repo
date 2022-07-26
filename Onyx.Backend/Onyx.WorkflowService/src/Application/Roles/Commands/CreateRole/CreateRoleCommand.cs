using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Onyx.WorkFlowService.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public decimal LoanTransactionLimit { get; set; }
        public int MaxLoanCountBooked { get; set; }
        public decimal MaxLoanVolumeBooked { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                //check userid only if default users exist for an organization, this would bypass validation of user on admin creation
                var checkRoles = await _context.Roles.Where(a=>a.OrganizationId == request.OrganizationId).ToListAsync();
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (checkRoles.Any())
                {
                    if (user.staff == null)
                    {
                        return Result.Failure(new string[] { "Unable to create role.Invalid User ID and Organization credentials!" });
                    }
                    else
                    {
                        createdBy = user.staff.StaffId;
                    }
                }
                else
                {
                    createdBy = user.staff?.Email??request.CreatedBy;
                }
              
                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Id == request.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Unable to create role. Staff does not belong to Organization" });
                }
                var roledetails = await _context.Roles.FirstOrDefaultAsync(a=>a.Name == request.Name && a.OrganizationId == request.OrganizationId);
                if (roledetails != null)
                {
                    return Result.Failure(new string[] { "Role details already exist" });
                }
                var role = new Role
                {
                    Name = request.Name.Trim(),
                    OrganizationId = request.OrganizationId,
                    AccessLevel = request.AccessLevel,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                    LoanTransactionLimit = request.LoanTransactionLimit,
                    MaxLoanCountBooked = request.MaxLoanCountBooked,
                    MaxLoanVolumeBooked = request.MaxLoanVolumeBooked,
                    Status = Status.Active
                };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success( "Role creation was successful", role );
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Role creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
