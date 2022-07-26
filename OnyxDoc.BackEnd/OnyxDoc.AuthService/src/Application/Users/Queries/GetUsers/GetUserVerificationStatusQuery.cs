using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUserVerificationStatusQuery : IRequest<Result>
    {

        public int SubscriberId { get; set; }

        public string UserId { get; set; }
    }

    public class GetUserVerificationStatusQueryHandler : IRequestHandler<GetUserVerificationStatusQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUserVerificationStatusQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUserVerificationStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.UserId.Trim());
                if (result.user == null)
                {
                    return Result.Failure(new string[] { $"User does not exist with Id: {request.UserId}" });
                }
                if (!result.user.EmailConfirmed)
                {
                    return Result.Failure("Email is not verified");
                }
                return Result.Success("Verified","Email is verified");
                

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving user by Id: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
