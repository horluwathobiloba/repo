using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.Login
{
    public partial class LoginCommand : UserAuth, IRequest<AuthResult>
    {
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public LoginCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authenticationService.Login(request.Email, request.Password);
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Error logging in : "+ ex?.Message ?? ex?.InnerException.Message }; 
            }
        }
    }
}
