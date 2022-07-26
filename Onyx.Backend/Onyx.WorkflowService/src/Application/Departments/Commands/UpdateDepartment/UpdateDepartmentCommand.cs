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

namespace Onyx.WorkFlowService.Application.Departments.Commands.UpdateDepartment
{
    public partial class UpdateDepartmentCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateDepartmentCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
              
                //check userid and organization
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to update department.Invalid User ID and Organization credentials!" });
                }
                var organization = await _context.Departments.FirstOrDefaultAsync(a => a.OrganizationId == request.OrganizationId);
                if (organization == null)
                {
                    return Result.Failure(new string[] { "Organization does not exist" });
                }

                var entity = await _context.Departments.FindAsync(request.DepartmentId);

                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid Department details" });
                }

                entity.Name = request.Name.Trim();
                entity.OrganizationId = request.OrganizationId;
                entity.Status = Status.Active;
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Department updated successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error updating department : ", ex?.Message ?? ex?.InnerException.Message });
            }
          
        }
    }
}
