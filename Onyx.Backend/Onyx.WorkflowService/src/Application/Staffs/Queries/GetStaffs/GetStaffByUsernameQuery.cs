using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs
{
    public class GetStaffByUsernameQuery : IRequest<Result>
    {
        public string  UserName { get; set; }
    }

    public class GetStaffByUsernameQueryHandler : IRequestHandler<GetStaffByUsernameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetStaffByUsernameQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetStaffByUsernameQuery request, CancellationToken cancellationToken)
        {
            var result = await  _identityService.GetUserByUsername(request.UserName);
            var staff = _mapper.Map<StaffDto>(result.staff);
            return Result.Success(staff);
        }
    }
}
