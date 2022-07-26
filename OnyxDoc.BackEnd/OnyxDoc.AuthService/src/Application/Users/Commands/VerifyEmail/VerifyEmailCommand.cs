
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AutoMapper;

namespace OnyxDoc.AuthService.Application.Users.Commands.VerifyEmail
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
                if (resultEmail.user == null)
                {
                    return Result.Failure("Invalid User");
                }
                var confirmResult = await _identityService.VerifyEmailAsync(resultEmail.user, resultEmail.user.Token);
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
  
