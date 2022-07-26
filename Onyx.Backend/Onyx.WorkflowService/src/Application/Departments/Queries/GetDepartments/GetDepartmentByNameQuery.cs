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
    public class GetDepartmentByNameQuery : IRequest<Result>
    {
        public string  Name { get; set; }
    }

    public class GetDepartmentByNameQueryHandler : IRequestHandler<GetDepartmentByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDepartmentByNameQuery request, CancellationToken cancellationToken)
        {
            var department = await  _context.Departments.FirstOrDefaultAsync(a=>a.Name == request.Name);
            var dept = _mapper.Map<DepartmentDto>(department);
            return Result.Success(dept);
            
        }
    }
}
