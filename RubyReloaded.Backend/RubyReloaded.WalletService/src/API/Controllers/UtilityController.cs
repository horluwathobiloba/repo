using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Domain.Enums;
using RubyReloaded.WalletService.Infrastructure.Utility;

namespace API.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UtilityController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public UtilityController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpGet("getcurrencies")]
        public async Task<ActionResult<Result>> GetCurrencies()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                 ((CurrencyCode[])Enum.GetValues(typeof(CurrencyCode))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get currency enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

       

        [HttpGet("getproductinterestintervals")]
        public async Task<ActionResult<Result>> GetProductInterestIntervals()
        {
            try
            { 
                return await Task.Run(() =>
                Result.Success(((ProductInterestInterval[])Enum.GetValues(typeof(ProductInterestInterval))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Product Interest Intervals failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

      

        [HttpGet("gettransactionservices")]
        public async Task<ActionResult<Result>> GetTransactionServices()
        {
            try
            {
                return await Task.Run(() => 
                Result.Success(((TransactionServiceType[])Enum.GetValues(typeof(TransactionServiceType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Transaction services failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getproductcategories")]
        public async Task<ActionResult<Result>> GetProductCategories()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ProductCategory[])Enum.GetValues(typeof(ProductCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Product Categories failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontributionfrequencies")]
        public async Task<ActionResult<Result>> GetContributionFrequencies()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ContributionFrequency[])Enum.GetValues(typeof(ContributionFrequency))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Contribution Frequencies failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getproductfundingcategories")]
        public async Task<ActionResult<Result>> GetProductFundingCategories()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ProductFundingCategory[])Enum.GetValues(typeof(ProductFundingCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Product Funding Categories failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getproductvariabletypes")]
        public async Task<ActionResult<Result>> GetProductVariableTypes()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((VariableType[])Enum.GetValues(typeof(VariableType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Product Variable Types failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpaymentchanneltypes")]
        public async Task<ActionResult<Result>> GetPaymentChannelTypes()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((PaymentChannelType[])Enum.GetValues(typeof(PaymentChannelType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Payment Channel Types failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getaccesslevels")]
        public async Task<ActionResult<Result>> GetAccessLevels()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((AccessLevel[])Enum.GetValues(typeof(AccessLevel))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getaccountclass")]
        public async Task<ActionResult<Result>> GetAccountClass()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((AccountClass[])Enum.GetValues(typeof(AccountClass))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getaccountfreezetype")]
        public async Task<ActionResult<Result>> GetAcountFreezeType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((AccountFreezeType[])Enum.GetValues(typeof(AccountFreezeType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getaccountstatus")]
        public async Task<ActionResult<Result>> GetAccountStatus()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((AccountStatus[])Enum.GetValues(typeof(AccountStatus))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getfundingsourcecategory")]
        public async Task<ActionResult<Result>> GetFundingSourceCategory()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((FundingSourceCategory[])Enum.GetValues(typeof(FundingSourceCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString()}).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

          [HttpGet("getbankservicecategory")]
        public async Task<ActionResult<Result>> GetBankServiceCategory()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((BankServiceCategory[])Enum.GetValues(typeof(BankServiceCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

          [HttpGet("getfeetype")]
        public async Task<ActionResult<Result>> GetFeeType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((FeeType[])Enum.GetValues(typeof(FeeType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

          [HttpGet("getduration")]
        public async Task<ActionResult<Result>> GetDuration()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((Duration[])Enum.GetValues(typeof(Duration))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

          [HttpGet("getinteresttype")]
        public async Task<ActionResult<Result>> GetInterestType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((InterestType[])Enum.GetValues(typeof(InterestType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getapplicationtype")]
        public async Task<ActionResult<Result>> GetApplicationType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ApplicationType[])Enum.GetValues(typeof(ApplicationType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getaccounttype")]
        public async Task<ActionResult<Result>> GetAccountType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((AccountType[])Enum.GetValues(typeof(AccountType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
         [HttpGet("getnotificationstatus")]
        public async Task<ActionResult<Result>> GetNotificationStatus()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((NotificationStatus[])Enum.GetValues(typeof(NotificationStatus))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
         /*[HttpGet("getpaymentchanneltypes")]
        public async Task<ActionResult<Result>> GetPaymentChannelTypes()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((PaymentChannelType[])Enum.GetValues(typeof(PaymentChannelType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }*/
         [HttpGet("getpaymentgatewaycategories")]
        public async Task<ActionResult<Result>> GetPaymentGatewayCategories()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((PaymentGatewayCategory[])Enum.GetValues(typeof(PaymentGatewayCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
         [HttpGet("getpaymentgatewayservicecategories")]
        public async Task<ActionResult<Result>> GetPaymentGatewayServiceCategories()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((PaymentGatewayServiceCategory[])Enum.GetValues(typeof(PaymentGatewayServiceCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
         [HttpGet("getpaymentstatuses")]
        public async Task<ActionResult<Result>> GetPaymentStatuses()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((PaymentStatus[])Enum.GetValues(typeof(PaymentStatus))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        


        [HttpGet("getstatuses")]
        public async Task<ActionResult<Result>> GetStatuses()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((Status[])Enum.GetValues(typeof(Status))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getservicetypes")]
        public async Task<ActionResult<Result>> GetServiceType()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ServiceType[])Enum.GetValues(typeof(ServiceType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getservicecategories")]
        public async Task<ActionResult<Result>> GetServiceCategory()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ServiceCategory[])Enum.GetValues(typeof(ServiceCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getresponsecodes")]
        public async Task<ActionResult<Result>> GetResponseCodes()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((ResponseCode[])Enum.GetValues(typeof(ResponseCode))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getwishliststatus")]
        public async Task<ActionResult<Result>> GetWishListStatus()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((WishListStatus[])Enum.GetValues(typeof(WishListStatus))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getwishistextensionfrequencies")]
        public async Task<ActionResult<Result>> GetWishlistExtensionFrequencies()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((WishlistExtensionFrequency[])Enum.GetValues(typeof(WishlistExtensionFrequency))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getwishcategories")]
        public async Task<ActionResult<Result>> GetWishCategories()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((WishCategory[])Enum.GetValues(typeof(WishCategory))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

      

        [HttpGet("gettransactiontypes")]
        public async Task<ActionResult<Result>> GetTransactionTypes()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((TransactionType[])Enum.GetValues(typeof(TransactionType))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("gettransactionstatuses")]
        public async Task<ActionResult<Result>> GetTransactionStatuses()
        {
            try
            {
                return await Task.Run(() =>
                Result.Success(((TransactionStatus[])Enum.GetValues(typeof(TransactionStatus))).
                Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Enum Values failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
      

    }
}