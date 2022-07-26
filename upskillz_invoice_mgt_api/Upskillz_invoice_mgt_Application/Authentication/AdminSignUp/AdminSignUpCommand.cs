using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.Common.Interfaces;
using Upskillz_invoice_mgt_Application.Common.Models;
using Upskillz_invoice_mgt_Domain.Entities;

namespace Upskillz_invoice_mgt_Application.Authentication.AdminSignUp
{
    public class AdminSignUpCommand : IRequest<Response<string>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public string BusinessName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AdminSignUpCommandHandler : IRequestHandler<AdminSignUpCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenConverter _tokenConverter;
        public AdminSignUpCommandHandler(IMapper mapper, UserManager<AppUser> userManager, ITokenConverter tokenConverter)
        {
            _mapper = mapper;
            _userManager = userManager;
            _tokenConverter = tokenConverter;
        }
        public async Task<Response<string>> Handle(AdminSignUpCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AppUser>(request);
            user.UserName = request.Email;
            user.IsActive = true;
            var response = new Response<string>();

            var result = await _userManager.CreateAsync(user, request.Password);
            var addRole = await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var res = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded && addRole.Succeeded && res.Succeeded)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Message = "SignUp Successful!";
                return response;
            }
            response.Message = GetErrors(result);
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            response.Message = "SignUp not successful";
            return response;

        }

        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }
    }
}
