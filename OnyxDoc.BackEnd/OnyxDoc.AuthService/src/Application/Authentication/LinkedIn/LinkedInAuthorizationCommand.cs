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
    public partial class LinkedInAuthorizationCommand : IRequest<Result>
    {
        public string State { get; set; }

        public string  Code { get; set; }
    }

    public class LinkedInAuthorizationCommandHandler : IRequestHandler<LinkedInAuthorizationCommand,Result>
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

        public LinkedInAuthorizationCommandHandler(IApplicationDbContext context, IIdentityService identityService,
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

        public async Task<Result> Handle(LinkedInAuthorizationCommand request, CancellationToken cancellationToken)
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
                    return Result.Failure(detail.error_description);
                }
                //get user's profile details
                var linkedInMemberProfile = _configuration["LinkedIn:GetLinkedInMemberProfile"];
                var memberProfile = await _apiClient.GetAPIUrl(linkedInMemberProfile, detail.access_token, "");
                Root profile = JsonConvert.DeserializeObject<Root>(memberProfile);
                //get user's email details
                var linkedInEmailUrl = _configuration["LinkedIn:GetLinkedInUserEmail"];
                var emailResponse = await _apiClient.GetAPIUrl(linkedInEmailUrl, detail.access_token, "");
                EmailRoot email = JsonConvert.DeserializeObject<EmailRoot>(emailResponse);

                if (profile == null || email == null)
                {
                    return Result.Failure("Invalid LinkedInDetails");
                }
                //sign and subscribe command
                var handler = new SignUpCommandHandler(_context, _identityService, _base64ToFileConverter, _emailService, _configuration,
                                          _sqlService, _stringHashingService, _generateUserInviteLinkService, _mapper, _authenticateService);

                var command = new SignUpCommand
                {
                    FirstName = profile.firstName.localized.en_US,
                    LastName = profile.lastName.localized.en_US,
                    ContactEmail = email.elements[0].Handle.emailAddress,
                    SubscriberType = SubscriberType.Individual,
                    Name = profile.firstName.localized.en_US,
                    SubscriberAccessLevel = SubscriberAccessLevel.Client,
                    ProfilePicture = profile.profilePicture.DisplayImage.elements[0].identifiers[0].identifier,
                    Country = profile.lastName.preferredLocale.country,
                    Email = email.elements[0].Handle.emailAddress
                };
                var response = await handler.Handle(command, cancellationToken);
                if (response.Succeeded)
                    return Result.Success(response.Entity);
                else
                    return Result.Failure(response?.Message??response?.Messages[0]);

            }
            catch (Exception ex)
            {
                return Result.Failure(" Error retrieving Linkedin Details: "+ ex?.Message ?? ex?.InnerException.Message); 
            }
        }
    }
}
