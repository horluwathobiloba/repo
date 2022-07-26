using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Infrastructure.Utility;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Utilities.Queries;
using RubyReloaded.AuthService.Domain.Enums;

namespace API.Controllers
{
    public class EnumController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        //public EnumController(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;

        //    accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
        //    if (accessToken == null)
        //    {
        //        throw new Exception("You are not authorized!");
        //    }
        //}


        [HttpGet("getaccesslevels")]
        public async Task<ActionResult<Result>> GetAccessLevels()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                  ((AccessLevel[])Enum.GetValues(typeof(AccessLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getcollectioncycles")]
        public async Task<ActionResult<Result>> GetCollectionCycles()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((CollectionCycle[])Enum.GetValues(typeof(CollectionCycle))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getcooperativeTypes")]
        public async Task<ActionResult<Result>> GetCooperativeTypes()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                 ((CooperativeType[])Enum.GetValues(typeof(CooperativeType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcurrencycodes")]
        public async Task<ActionResult<Result>> GetCurrencyCodes()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((CurrencyCode[])Enum.GetValues(typeof(CurrencyCode))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getexploreimagetype")]
        public async Task<ActionResult<Result>> GetExploreImageType()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((ExploreImageType[])Enum.GetValues(typeof(ExploreImageType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] {"retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getgenders")]
        public async Task<ActionResult<Result>> GetGenders()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((Gender[])Enum.GetValues(typeof(Gender))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getlinktype")]
        public async Task<ActionResult<Result>> GetLinkType()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((LinkType[])Enum.GetValues(typeof(LinkType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getpasswordresetstatus")]
        public async Task<ActionResult<Result>> GetPasswordResetStatus()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                     ((PasswordResetStatus[])Enum.GetValues(typeof(PasswordResetStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                     ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getadminpermissions")]
        public async Task<ActionResult<Result>> GetAdminPermmissions()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((AdminPermissions[])Enum.GetValues(typeof(AdminPermissions))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getexternaluserpermissions")]
        public async Task<ActionResult<Result>> GetExternalUserPermmissions()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((ExternalUserPermissions[])Enum.GetValues(typeof(ExternalUserPermissions))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getpoweruserpermissions")]
        public async Task<ActionResult<Result>> GetPowerUserPermmissions()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((PowerUsersPermissions[])Enum.GetValues(typeof(PowerUsersPermissions))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
          [HttpGet("getsuperadminpermissions")]
        public async Task<ActionResult<Result>> GetSuperAdminPermmissions()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((SuperAdminPermissions[])Enum.GetValues(typeof(SuperAdminPermissions))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getsupportpermissions")]
        public async Task<ActionResult<Result>> GetSupportPermmissions()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((SupportPermissions[])Enum.GetValues(typeof(SupportPermissions))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getstatuses")]
        public async Task<ActionResult<Result>> GetStatues()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                    ((Status[])Enum.GetValues(typeof(Status))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getrolecategories")]
        public async Task<ActionResult<Result>> GetRoleCategories()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                      ((RoleCategory[])Enum.GetValues(typeof(RoleCategory))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                      ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getrequesttypes")]
        public async Task<ActionResult<Result>> GetRequestTypes()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                                  ((RequestType[])Enum.GetValues(typeof(RequestType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getproductcategory")]
        public async Task<ActionResult<Result>> GetProductCategory()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                  ((ProductCategory[])Enum.GetValues(typeof(ProductCategory))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getpriorities")]
        public async Task<ActionResult<Result>> GetPriorities()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                     ((PriorityLevel[])Enum.GetValues(typeof(PriorityLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                     ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getsubscriptiontype")]
        public async Task<ActionResult<Result>> GetSubscriptionTypes()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((SubscriptionType[])Enum.GetValues(typeof(SubscriptionType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getuseraccesslevels")]
        public async Task<ActionResult<Result>> GetUserAccessLevels()
        {
            try 
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                   ((UserAccessLevel[])Enum.GetValues(typeof(UserAccessLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getworkflowcategory")]
        public async Task<ActionResult<Result>> GetWorflowCategory()
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Task.Run(() => Result.Success(
                  ((WorkflowUserCategory[])Enum.GetValues(typeof(WorkflowUserCategory))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
       
    }

}
