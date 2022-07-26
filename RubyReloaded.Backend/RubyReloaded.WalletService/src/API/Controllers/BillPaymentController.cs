using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Airtime.Queries;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.WalletService.Application.BillPayments.Queries.DataServices.Queries;
using RubyReloaded.WalletService.Application.BillPayments.Queries.ElectricityServices;
using RubyReloaded.WalletService.Application.BillPayments.Queries;
using RubyReloaded.WalletService.Application.BillPayments.Queries.GetBillPaymentCategoryOptions;
using RubyReloaded.WalletService.Application.BillPayments.Queries.GetBillPaymentFields;
using RubyReloaded.WalletService.Application.MakePayment.Commands;
using RubyReloaded.WalletService.Application.BillPayments.Commands.Validate;

namespace API.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BillPaymentController: ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BillPaymentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpGet("getairtimepaymentfields/{airtimecategory}/{userid}")]
        public async Task<ActionResult<Result>> GetAirtimePaymentFields(int airtimecategory,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetAirtimePaymentMapping {AirtimeCategory=airtimecategory });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getairtimecategory/{userid}")]
        public async Task<ActionResult<Result>> GetAirtimeCategory(string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetAirtimeCategory());
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        //[HttpGet("getdataservicecategory/{categoryid}")]
        //public async Task<ActionResult<Result>> GetDataServiceCategory(int categoryid)
        //{
        //    try
        //    {
        //        accessToken.ValidateToken(command.UserId);
        //        return await Mediator.Send(new GetDataServicesPaymentMapping { CategoryId = categoryid });
        //    }
        //    catch (ValidationException ex)
        //    {

        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}


        //[HttpGet("getdataservicescategory")]
        //public async Task<ActionResult<Result>> GetDataServicesCategories()
        //{
        //    try
        //    {
        //        accessToken.ValidateToken(command.UserId);
        //        return await Mediator.Send(new GetDataServicesCategoriesQuery());
        //    }
        //    catch (ValidationException ex)
        //    {

        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}




        //[HttpGet("getelectricityservicecategory/{categoryid}")]
        //public async Task<ActionResult<Result>> GetElectricityServiceCategory(int categoryid)
        //{
        //    try
        //    {
        //       accessToken.ValidateToken(command.UserId);
        //        return await Mediator.Send(new GetElectricityPaymentMapping { CategoryId = categoryid });
        //    }
        //    catch (ValidationException ex)
        //    {

        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}


        //[HttpGet("getelectricityservicescategory")]
        //public async Task<ActionResult<Result>> GetElectricityServicesCategories()
        //{
        //    try
        //    {
        //        accessToken.ValidateToken(command.UserId);
        //        return await Mediator.Send(new GetElectricityServiceCategoriesQuery());
        //    }
        //    catch (ValidationException ex)
        //    {

        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}



        [HttpGet("getcategories/{userid}")]
        public async Task<ActionResult<Result>> GetWebCategories(string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetWebCategoriesQuery());
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbillpaymentcategoryoptions/{categoryid}/{userid}")]
        public async Task<ActionResult<Result>> GetBillPaymentCategoryOptions(int categoryid, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetBillPaymentCategoryOptionsQuery { 
                CategoryId=categoryid
                });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getbillpaymentcategoryfields/{billid}/{userid}")]
        public async Task<ActionResult<Result>> GetBillPaymentCategoryFields(int billid,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetBillPaymentFieldsQuery
                {
                    BillId = billid
                });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("makepayment")]
        public async Task<ActionResult<Result>> MakePayment(MakePaymentCommand command)
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

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("validate")]
        public async Task<ActionResult<Result>> Validate(ValidateCommand command)
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

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
