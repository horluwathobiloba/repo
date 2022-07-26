using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs
{
    public class GetStaffsByDepartmentIdQuery : IRequest<Result>
    {
        public int DepartmentId { get; set; }
    }

    public class GetStaffsByDepartmentIdQueryHandler : IRequestHandler<GetStaffsByDepartmentIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetStaffsByDepartmentIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetStaffsByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetStaffsByDepartmentId(request.DepartmentId);
                var staffList = _mapper.Map<List<StaffListDto>>(result.staffs);
                return Result.Success(staffList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
