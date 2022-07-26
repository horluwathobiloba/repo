using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.ChangeProductCategoryConfigurationStatus;
using RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.CreateProductCategoryConfiguration;
using RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.UpdateProductCategoryConfigurations;
using RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Queries.GetProductCategoryConfiguration;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductCategoryConfigurationController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ProductCategoryConfigurationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateProductCategoryConfigurationCommand command)
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

                return Result.Failure(new string[] { "Produt Category configuration was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changeproductcategorystatus")]
        public async Task<ActionResult<Result>> ChangeProductCategorytatus(ChangeProductCategoryConfigurationStatusCommand command)
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

                return Result.Failure(new string[] { "Changing Produt Category Configuration Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateProductCategoryConfigurationCommand command)
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

                return Result.Failure(new string[] { "Product Category Configuration update was not successful" + ex?.Message ?? ex?.InnerException?.Message });

            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetProductCategoryConfigurationQuery { UserId = userId});
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Produt Category Configuration retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetByProdutCategoryConfigurationId(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetProductCategoryConfigurationByIdQuery {UserId = userId, ProductCategoryConfigurationId = id });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Produt Category Configuration retrieval by Produt Category Configuration id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
