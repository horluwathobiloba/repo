using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Onyx.WorkFlowService.Application.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public List<string> Names { get; set; }
    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateDepartmentCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check userid and organization
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to create department.Invalid User ID and Organization credentials!" });
                }
                //check department name first
                var departmentNames = await _context.Departments.Where(a => a.OrganizationId == request.OrganizationId).ToDictionaryAsync(a=>a.Name);
                foreach (var name in request.Names)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }
                    if (departmentNames != null)
                    {
                        departmentNames.TryGetValue(name, out Department department);
                        if (request.Names.Count == 1 && department != null)
                            return Result.Failure(new string[] { "Department already exists with this detail" });
                        if (request.Names.Count > 1 && department != null)
                            continue;
                    }
                    var entity = new Department
                    {
                        Name = name.Trim(),
                        OrganizationId = request.OrganizationId,
                        Status = Status.Active,
                        CreatedBy = request.UserId,
                         CreatedDate = DateTime.Now

                    };
                   await _context.Departments.AddAsync(entity);
                }
               
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Department was created successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Department creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
