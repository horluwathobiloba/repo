using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.Products.Commands.UpdateProduct;
using RubyReloaded.WalletService.Application.Products.Commands.ChangeProductStatus;
using RubyReloaded.WalletService.Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Http;
using RubyReloaded.WalletService.Infrastructure.Utility;
using RubyReloaded.WalletService.Application.Products.Queries.GetProducts;
using RubyReloaded.WalletService.Application.ProductConfigurations.Queries.GetProducts;
using RubyReloaded.WalletService.Application.Products.Commands.CreateWishlist;
using RubyReloaded.WalletService.Application.Products.Commands.UpdateWishlist;
using RubyReloaded.WalletService.Application.Products.Queries.GetWishlist;
using RubyReloaded.WalletService.Application.Services.Commands.ChangeWishlistStatus;
using RubyReloaded.WalletService.Application.Products.Commands.ExtendWishlist;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WishlistsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WishlistsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWishlistCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wishlist creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateWishlistCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wishlist update was not successful" + ex?.Message ?? ex?.InnerException?.Message });

            }
        }

        [HttpPost("changewishliststatus")]
        public async Task<ActionResult<Result>> ChangeWishlistStatus(ChangeWishlistStatusCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Changing Wishlist Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("extendwishlist")]
        public async Task<ActionResult<Result>> ExtendWishlist(ExtendWishlistCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Extending Wishlist was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWishlistsQuery { UserId = userId});
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Wislists retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetById(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWishlistByIdQuery {UserId = userId, Id = id });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Wislist retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getactivewishlists/{userId}")]
        public async Task<ActionResult<Result>> GetActiveWislists(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetActiveWishlistsQuery { UserId = userId});
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Active Wishlists retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
