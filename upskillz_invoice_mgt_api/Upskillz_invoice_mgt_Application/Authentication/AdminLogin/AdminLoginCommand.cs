using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Application.Authentication.AdminLogin.LoginDto;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.Common.Interfaces;
using Upskillz_invoice_mgt_Domain.Entities;

namespace Upskillz_invoice_mgt_Application.Authentication.AdminLogin
{
    public class AdminLoginCommand : IRequest<Response<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, Response<LoginResponseDto>>
    {
        //private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly ITokenGeneratorService _tokenGenerator;

        public AdminLoginCommandHandler(UserManager<AppUser> userManager, IIdentityService identityService, ITokenGeneratorService tokenGenerator)
        {
            //_logger = logger;
            _userManager = userManager;
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<Response<LoginResponseDto>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<LoginResponseDto>();
            //_logger.LogInformation("Login Attempt");
            var validityResult = await _identityService.ValidateUser(request.Email, request.Password);

            if (!validityResult.Succeeded)
            {
                //_logger.LogError("Login operation failed");
                response.Message = validityResult.Message;
                response.StatusCode = validityResult.StatusCode;
                response.Succeeded = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            var result = new LoginResponseDto()
            {
                Id = user.Id,
                Token = await _tokenGenerator.GenerateToken(user),
            };

            await _userManager.UpdateAsync(user);

            //_logger.LogInformation("User successfully logged in");
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Login Successfully";
            response.Data = result;
            response.Succeeded = true;
            return response;
        }
    }
}
