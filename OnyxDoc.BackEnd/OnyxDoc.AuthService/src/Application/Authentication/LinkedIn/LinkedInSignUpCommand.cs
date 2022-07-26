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
using Microsoft.Extensions.Configuration;
using System.Web;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.Login
{
    public partial class LinkedInSignUpCommand : IRequest<Result>
    {
        
    }

    public class LinkedInSignUpCommandHandler : IRequestHandler<LinkedInSignUpCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IAPIClient _apiClient;

        public LinkedInSignUpCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService, IConfiguration configuration, IAPIClient apiClient)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _apiClient = apiClient;
        }

        public async Task<Result> Handle(LinkedInSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var linkedIn = _configuration["LinkedIn:LinkedInRedirectUrl"];
                var redirectUrl = HttpUtility.UrlEncode(linkedIn);
                var linkedInUrl = _configuration["LinkedIn:LinkedInSignUpUrl"] + "&client_id=" + _configuration["LinkedIn:Authentication:LinkedIn:ClientId"] + "&state="+Guid.NewGuid()
                                  +"&redirect_uri=" + redirectUrl;
                return Result.Success("LinkedIn Redirect Url",linkedInUrl);
            }
            catch (Exception ex)
            {
                return Result.Failure(" Error signing up with Linkedin : "+ ex?.Message ?? ex?.InnerException.Message); 
            }
        }
    }
}
