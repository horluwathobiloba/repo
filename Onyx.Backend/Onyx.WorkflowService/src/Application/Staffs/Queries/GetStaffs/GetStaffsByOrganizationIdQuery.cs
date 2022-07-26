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
    public class GetStaffsByOrganizationIdQuery : IRequest<Result>
    {
        public int OrganizationId { get; set; }
    }

    public class GetStaffsByOrganizationIdQueryHandler : IRequestHandler<GetStaffsByOrganizationIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetStaffsByOrganizationIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetStaffsByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetStaffsByOrganizationId(request.OrganizationId);
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
