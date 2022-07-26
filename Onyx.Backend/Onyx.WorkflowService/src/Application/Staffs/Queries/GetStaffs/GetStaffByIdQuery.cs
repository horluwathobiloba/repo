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
    public class GetStaffByIdQuery : IRequest<Result>
    {
        public string  Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetStaffByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.Id);
                var staff = _mapper.Map<StaffDto>(result.staff);
                return Result.Success(staff);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
