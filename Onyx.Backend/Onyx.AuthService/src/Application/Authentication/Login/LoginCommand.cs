using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Onyx.AuthService.Application.Authentication.Commands.Login
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
                if (string.IsNullOrWhiteSpace(request.OrganizationCode))
                {
                    return new AuthResult { IsSuccess = false, Message = "Invalid Organization Code" };
                }
                var organization = await _context.Organizations.FirstOrDefaultAsync(a => a.Code == request.OrganizationCode);
                if (organization == null)
                {
                    return new AuthResult { IsSuccess = false, Message = "Invalid Organization Code" };
                }
                return await _authenticationService.Login(request.Email, request.Password, organization.Id);
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Error logging in : "+ ex?.Message +  ex?.InnerException.Message }; 
            }
        }
    }
}
