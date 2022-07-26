using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using System;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.LogOut
{
    public partial class LogoutCommand :  IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public LogoutCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authenticationService.LogOut(request.Email);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { " Error logging out : " + ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}