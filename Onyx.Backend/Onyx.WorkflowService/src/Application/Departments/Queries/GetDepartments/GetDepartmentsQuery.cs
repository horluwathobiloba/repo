using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentsQuery : IRequest<Result>
    {
    }

    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                var dept = _mapper.Map<List<DepartmentListDto>>(departments);
                return Result.Success(dept);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
