using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models; 
using OnyxDoc.FormBuilderService.Application.Common.Exceptions; 
/*using OnyxDoc.FormBuilderService.Application.Utilities.Queries;*/
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Linq;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UtilityController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public UtilityController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            //accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            //if (accessToken == null)
            //{
            //    throw new Exception("You are not authorized!");
            //}
        }

        [HttpGet("getblockcontrols")]
        public async Task<ActionResult<Result>> GetBlockControls()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                 ((BlockControlType[])Enum.GetValues(typeof(BlockControlType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get Block Controls failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolpropertytypes")]
        public async Task<ActionResult<Result>> GetControlPropertyTypes()
        {
            try
            { 
                return await Task.Run(() => Result.Success(
                  ((ControlPropertyType[])Enum.GetValues(typeof(ControlPropertyType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control property type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolpropertyvaluetypes")]
        public async Task<ActionResult<Result>> GetControlPropertyValueTypes()
        {
            try
            { 
                return await Task.Run(() => Result.Success(
                  ((ControlPropertyValueType[])Enum.GetValues(typeof(ControlPropertyValueType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control property value type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontroltypes")]
        public async Task<ActionResult<Result>> GetControlTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((ControlType[])Enum.GetValues(typeof(ControlType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getfieldcontrols")]
        public async Task<ActionResult<Result>> GetFieldControls()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((FieldControlType[])Enum.GetValues(typeof(FieldControlType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get field control enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getinputvaluetypes")]
        public async Task<ActionResult<Result>> GetInputValueTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((InputValueType[])Enum.GetValues(typeof(InputValueType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get field value types enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getpagelayouts")]
        public async Task<ActionResult<Result>> GetPageLayouts()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((PageLayout[])Enum.GetValues(typeof(PageLayout))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page layout enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getdimensiontypes")]
        public async Task<ActionResult<Result>> GetDimensionTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((DimensionType[])Enum.GetValues(typeof(DimensionType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get dimension type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getimagesizes")]
        public async Task<ActionResult<Result>> GetImageSizes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((ImageSize[])Enum.GetValues(typeof(ImageSize))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get image size enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getorientationtypes")]
        public async Task<ActionResult<Result>> GetOrientationTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((OrientationType[])Enum.GetValues(typeof(OrientationType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get orientation type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getpagesizes")]
        public async Task<ActionResult<Result>> GetPageSizes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((PageSize[])Enum.GetValues(typeof(PageSize))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page size enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getaccesslevels")]
        public async Task<ActionResult<Result>> GetAccessLevels()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((AccessLevel[])Enum.GetValues(typeof(AccessLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get access levels enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getdocumentsharetypes")]
        public async Task<ActionResult<Result>> GetDocumentShareTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((DocumentShareType[])Enum.GetValues(typeof(DocumentShareType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get document share types enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getdocumentstatuses")]
        public async Task<ActionResult<Result>> GetDocumentStatuses()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((DocumentStatus[])Enum.GetValues(typeof(DocumentStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get document status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getdocumenttypes")]
        public async Task<ActionResult<Result>> GetDocumentTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((DocumentType[])Enum.GetValues(typeof(DocumentType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get document type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getreminderfrequencies")]
        public async Task<ActionResult<Result>> GetReminderFrequencies()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((ReminderFrequency[])Enum.GetValues(typeof(ReminderFrequency))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get access levels enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getroleaccesslevels")]
        public async Task<ActionResult<Result>> GetRoleAccessLevels()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((RoleAccessLevel[])Enum.GetValues(typeof(RoleAccessLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get role access levels enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getstatuses")]
        public async Task<ActionResult<Result>> GetStatuses()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((Status[])Enum.GetValues(typeof(Status))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getsubscriberaccesslevels")]
        public async Task<ActionResult<Result>> GetSubscriberAccessLevels()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((SubscriberAccessLevel[])Enum.GetValues(typeof(SubscriberAccessLevel))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscribers access levels enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getsubscribertypes")]
        public async Task<ActionResult<Result>> GetSubscriberTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((SubscriberType[])Enum.GetValues(typeof(SubscriberType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscriber types enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getweekofmonths")]
        public async Task<ActionResult<Result>> GetWeekOfMonths()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((WeekOfMonth[])Enum.GetValues(typeof(WeekOfMonth))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get access levels enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getworkflowusercategories")]
        public async Task<ActionResult<Result>> GetWorkflowUserCategories()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((WorkflowUserCategory[])Enum.GetValues(typeof(WorkflowUserCategory))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get workflow user enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        } 
    }
}