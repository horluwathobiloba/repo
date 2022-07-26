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
    public class GetStaffsQuery : IRequest<Result>
    {
    }

    public class GetStaffsQueryHandler : IRequestHandler<GetStaffsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetStaffsQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetStaffsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetAll();
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
