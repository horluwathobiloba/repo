using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RubyReloaded.AuthService.Application.Authentication.Commands.Login
{
    public partial class LoginCommand : UserAuth, IRequest<AuthResult>
    {
        
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        private readonly IAuthenticateService _authenticationService;

        public LoginCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService, IIdentityService identityService)
        {
            _context = context;
            _authenticationService = authenticationService;
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.user == null) return new AuthResult { IsSuccess= false, Message = "Invalid email or password. Please check your login details." };
               
                var cooperativeIds = await _context.CooperativeMembers.
                    Where(x=>x.Email==user.user.Email  /*&& x.CooperativeAccessStatus == CooperativeAccessStatus.Approved*/)
                    .Select(x => x.CooperativeId)
                    .ToListAsync();
                
                var ajoids = await _context.AjoMembers.
                    Where(x => x.Email == user.user.Email)
                    .Select(x => x.AjoId)
                    .ToListAsync();
                

                return await _authenticationService.Login(request.Email, request.Password,cooperativeIds,ajoids); 
            }
            catch (Exception ex)
            {
                // we need to consider logging
                return new AuthResult { IsSuccess = false, Message = " Error logging in : "+ ex?.Message ?? ex?.InnerException.Message }; 
            }
        }
    }
}
