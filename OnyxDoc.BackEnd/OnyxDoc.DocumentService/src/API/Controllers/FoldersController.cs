using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Commands.ArchiveFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.DeleteFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.DuplicateFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.EditFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.MoveFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.UnArchiveFolder;
using OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders;
using OnyxDoc.DocumentService.Application.FolderShareDetails.Commands.DeleteFolderShareDetail;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FoldersController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public FoldersController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("createfolder")]
        public async Task<ActionResult<Result>> Create([FromBody]CreateFolderCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Folder Creation was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        //[HttpPost("createfolders")]
        //public async Task<ActionResult<Result>> CreateFolders(CreateFoldersCommand command)
        //{
        //    try
        //    {
        //        command.AccessToken = accessToken;
        //        return await Mediator.Send(command);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Result.Failure($"Folders Creation was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
        //    }
        //}


        [HttpGet("getbyid/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id, int subscriberId, string userId)
        {
            try
            {

                return await Mediator.Send(new GetFolderByIdQuery { Id = id, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving folder by Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getarchivedfolderbyid/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetArchivedFolderById(int id, int subscriberId, string userId)
        {
            try
            {

                return await Mediator.Send(new GetArchivedFolderByIdQuery { FolderId = id, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving archived folder by Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getarchivedfolders/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetArchivedFolders( int subscriberId, string userId)
        {
            try
            {

                return await Mediator.Send(new GetArchivedFoldersQuery { SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving archived folders  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getfoldersbyuserid/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetFoldersByUserId(int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new GetFoldersByUserIdQuery { SubscriberId = subscriberId, UserId = userId, AuthToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving folder by User Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getchildrenfiles/{parentFolderId}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetChildrenFiles(int parentFolderId, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new GetChildrenFilesQuery { ParentFolderId = parentFolderId, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving children files  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("editfolder")]
        public async Task<ActionResult<Result>> EditFolder(EditFolderCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Folder edit was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("movefolder")]
        public async Task<ActionResult<Result>> MoveFolder(MoveFolderCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Moving folder was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("archivefolder/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> ArchiveFolder(int id, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new ArchiveFolderCommand { Id = id, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Archiving folder was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        } 
        
        [HttpPost("unarchivefolder/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> UnArchiveFolder(int id, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new UnArchiveFolderCommand { Id = id, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Unarchiving folder was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }




        [HttpPost("duplicatefolder/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> DuplicateFolder(int id, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new DuplicateFolderCommand {Id = id ,SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Duplicating folder was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletefolder/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> DeleteFolder(int id, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new DeleteFolderCommand { Id = id,  SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Deleting folder was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
        
        
        [HttpDelete("deletefoldersharedetail/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> DeleteFolderShareDetail(int id, int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new DeleteFolderShareDetailCommand { Id = id,  SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Deleting folder shared detail was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }
}
