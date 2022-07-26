using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Entities;

namespace Upskillz_invoice_mgt_Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response<string>> GetUserNameAsync(string userId)
        {
            try
            {
                var resut = new Response<string>();
                var user = await _userManager.Users.FirstAsync(x => x.Id == userId);
                if (user != null)
                {
                    resut.Data = user.UserName;
                    resut.Message = $"Hey! it's {user.UserName}";
                    resut.Succeeded = true;
                    resut.StatusCode = (int)HttpStatusCode.OK;
                    return resut;
                }
                resut.Data = null;
                resut.Message = $"User with id {userId} is not existing";
                resut.Succeeded = false;
                resut.StatusCode = (int)HttpStatusCode.BadRequest;
                return resut;
            }
            catch (Exception ex)
            {
                return new Response<string>()
                {
                    Data = ex.Message,
                    Message = ex.ToString(),
                    Succeeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<bool>> ValidateUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var response = new Response<bool>();
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                response.Message = "Invalid Credentials";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            if (!await _userManager.IsEmailConfirmedAsync(user) && user.IsActive)
            {
                response.Message = "Account not activated";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                return response;
            }
            else
            {
                response.Succeeded = true;
                return response;
            }
        }
    }
}
