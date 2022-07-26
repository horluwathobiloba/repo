using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.Login
{
    public partial class LoginWithThirdPartyCommand : IRequest<AuthResult>
    {
        public string Email { get; set; }
        public ThirdPartyType ThirdPartyType { get; set; }
    }

    public class LoginWithThirdPartyCommandHandler : IRequestHandler<LoginWithThirdPartyCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public LoginWithThirdPartyCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<AuthResult> Handle(LoginWithThirdPartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authenticationService.LoginWithThirdParty(request.Email, request.ThirdPartyType);
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Error logging in : "+ ex?.Message ?? ex?.InnerException.Message }; 
            }
        }
    }
}
