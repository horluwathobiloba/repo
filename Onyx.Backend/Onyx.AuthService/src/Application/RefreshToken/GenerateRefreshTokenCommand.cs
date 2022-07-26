using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.RefreshToken
{
    public class GenerateRefreshTokenCommand: IRequest<AuthResult>
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
    public class GenerateRefreshTokenCommandHandler : IRequestHandler<GenerateRefreshTokenCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        public GenerateRefreshTokenCommandHandler(IApplicationDbContext context, IAuthenticateService authenticateService,ITokenService tokenService, IIdentityService identityService)
        {
            _context = context;
            _authenticateService = authenticateService;
            _identityService = identityService;
            _tokenService = tokenService;
        }
        public async Task<AuthResult> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var authResult = new AuthResult();
            try
            {
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.staff==null)
                {
                    authResult.Message = "User does not exist";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                //checks user
                var userToken = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.Email == request.Email && x.RefreshTokens == request.RefreshToken && DateTime.Now <= x.RefreshTokenExpires);
                if (userToken == null)
                {
                    authResult.Message = "Refresh token does not exist or refresh token time has expired, the user should login again.";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                //decode the refreshToken
                var decodedToken = await _tokenService.DecodeRefreshToken(request.RefreshToken);
                if (decodedToken == null)
                {
                    authResult.Message = "Couldn't decode refresh token";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                //call the login method and log in the user
                var loginUser = await _authenticateService.Login(decodedToken.UserName, decodedToken.Password, user.staff.OrganizationId);
                if (loginUser==null)
                {
                    authResult.Message = "Login failed";
                    authResult.IsSuccess = false;
                    return authResult;
                }
                authResult.Message = "Refresh token successful, new token generated";
                authResult.IsSuccess = true;
                authResult.Token = loginUser.Token;
                return authResult;
            }
            catch (Exception ex)
            {
                authResult.Message = "Error logging in : " + ex?.Message + ex?.InnerException.Message;
                authResult.IsSuccess = false;
                return authResult;
            }
        }
    }
}
