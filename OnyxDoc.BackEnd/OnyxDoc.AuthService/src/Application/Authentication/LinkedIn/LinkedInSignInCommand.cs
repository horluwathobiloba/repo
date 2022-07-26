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
using Newtonsoft.Json;
using OnyxDoc.AuthService.Application.Commands.SignUp;
using AutoMapper;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.Login
{
    public partial class LinkedInSignInCommand : IRequest<AuthResult>
    {
        public string State { get; set; }

        public string  Code { get; set; }
    }

    public class LinkedInSignInCommandHandler : IRequestHandler<LinkedInSignInCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IAPIClient _apiClient;
        private readonly IEmailService _emailService;
        private readonly ISqlService _sqlService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;

        public LinkedInSignInCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IBase64ToFileConverter fileConverter, IAPIClient apiClient, IEmailService emailService, IConfiguration configuration,
            ISqlService sqlService, IStringHashingService stringHashingService,
            IGenerateUserInviteLinkService generateUserInviteLinkService,
            IBase64ToFileConverter base64ToFileConverter, IMapper mapper, IAuthenticateService authenticateService)
        {
            _context = context;
            _generateUserInviteLinkService = generateUserInviteLinkService;
            _authenticateService = authenticateService;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
            _apiClient = apiClient;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _sqlService = sqlService;
            _stringHashingService = stringHashingService;
        }

        public async Task<AuthResult> Handle(LinkedInSignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var linkedIn = _configuration["LinkedIn:LinkedInRedirectUrl"];
                var redirectUrl = HttpUtility.UrlEncode(linkedIn);
                var authorizationUrl = _configuration["LinkedIn:LinkedInAuthorization"] + request.Code + "&redirect_uri=" + redirectUrl + "&client_id="
                                     + _configuration["LinkedIn:Authentication:LinkedIn:ClientId"] + "&client_secret=" + _configuration["LinkedIn:Authentication:LinkedIn:ClientSecret"];
                var result = await _apiClient.GetAPIUrl(authorizationUrl, "", "");
                var detail = JsonConvert.DeserializeObject<LinkedInViewModel>(result);
                if (!string.IsNullOrEmpty(detail.error))
                {
                    return new AuthResult { IsSuccess = false, Message = " Error logging in : " + detail.error_description };
                }
                //get user's email details
                var linkedInEmailUrl = _configuration["LinkedIn:GetLinkedInUserEmail"];
                var emailResponse = await _apiClient.GetAPIUrl(linkedInEmailUrl, detail.access_token, "");
                EmailRoot email = JsonConvert.DeserializeObject<EmailRoot>(emailResponse);

                var linkedInLoginRequest = new LoginWithThirdPartyCommandHandler(_context, _authenticateService);
                var command = new LoginWithThirdPartyCommand
                {
                    Email = email.elements[0].Handle.emailAddress,
                     ThirdPartyType = ThirdPartyType.Linkedln
                };
                return await linkedInLoginRequest.Handle(command, cancellationToken);
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Error logging in : " + ex?.Message ?? ex?.InnerException.Message };
            }
        }
    }
}
