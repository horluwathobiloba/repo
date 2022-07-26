using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using System.Collections.Generic;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentsByOrganizationIdQuery : IRequest<Result>
    {
        public int  OrganizationId { get; set; }
    }

    public class GetDepartmentsByOrganizationIdQueryHandler : IRequestHandler<GetDepartmentsByOrganizationIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentsByOrganizationIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDepartmentsByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _context.Departments.Where(a=>a.OrganizationId == request.OrganizationId).ToListAsync();
                var deptList = _mapper.Map<List<DepartmentListDto>>(departments);
                return Result.Success(deptList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
