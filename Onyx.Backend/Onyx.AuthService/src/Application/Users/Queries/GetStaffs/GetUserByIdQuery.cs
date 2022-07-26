using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;

namespace Onyx.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUserByIdQuery : IRequest<Result>
    {
        public string  Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result>
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

        public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.Id.Trim());
                var staff = _mapper.Map<UserDto>(result.staff);
                if (staff == null)
                {
                    return Result.Failure(new string[] { $"User does not exist with Id: {request.Id}" });
                }
                staff.Role = await _context.Roles.Where(a => a.Id == staff.RoleId).FirstOrDefaultAsync();
                staff.Organization = await _context.Organizations.Where(a => a.Id == staff.OrganizationId).FirstOrDefaultAsync();
                staff.JobFunction = await _context.JobFunctions.Where(a=>a.Id == staff.JobFunctionId).FirstOrDefaultAsync();
                return Result.Success(staff);
                

            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving staff by Id: "+ ex?.Message ?? ex?.InnerException?.Message );
            }
        }
    }
}
