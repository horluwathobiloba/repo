using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentByIdQuery : IRequest<Result>
    {
        public int  Id { get; set; }
    }

    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _context.Departments.FindAsync(request.Id);
                var deptList = _mapper.Map<DepartmentDto>(department);
                return Result.Success(deptList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
