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

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateProductCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Product creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateProductCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Product update was not successful" + ex?.Message ?? ex?.InnerException?.Message });

            }
        }

        [HttpPost("changeproductstatus")]
        public async Task<ActionResult<Result>> ChangeProductStatus(ChangeProductStatusCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Changing Product Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetProductsQuery { UserId = userId});
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Products retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetById(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetProductByIdQuery {UserId = userId, Id = id });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Product retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getactiveproducts/{userId}")]
        public async Task<ActionResult<Result>> GetActiveProducts(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetActiveProductsQuery { });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Active product retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
