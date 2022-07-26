
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AutoMapper;

namespace Onyx.AuthService.Application.Customers.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }

    }

    public class GetVerifyEmailHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetVerifyEmailHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resultEmail = await _identityService.GetUserByEmail(request.Email);
                if (resultEmail.staff == null)
                {
                    return Result.Failure("Invalid User");
                }
                var confirmResult = await _identityService.VerifyEmailAsync(resultEmail.staff, resultEmail.staff.Token);
                if (!confirmResult.Succeeded)
                {
                    return Result.Failure( confirmResult.Message);
                }
                return Result.Success(confirmResult.Messages[0]);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error verifying customer", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
  
