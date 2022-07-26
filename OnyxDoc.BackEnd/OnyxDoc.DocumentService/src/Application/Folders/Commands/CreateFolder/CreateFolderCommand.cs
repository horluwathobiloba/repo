using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder
{
    public class CreateFolderCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int RootFolderId { get; set; }
        public int ParentFolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public FolderType FolderType { get; set; }
        public List<CreateFolderRecipientRequest> CreateFolderRecipientRequests { get; set; }
        //public List<Domain.Entities.Document> Documents { get; set; }



        public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Result>
        {
            private readonly IApplicationDbContext _context;
            private readonly IAuthService _authService;
            private readonly IConfiguration _configuration;

            public CreateFolderCommandHandler(IApplicationDbContext context, IAuthService authService, IConfiguration configuration)
            {
                _context = context;
                _authService = authService;
                _configuration = configuration;
            }
            public async Task<Result> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                    if (!isValidSubscriber)
                    {
                        return Result.Failure("You are not a valid subscriber");
                    }
                    var subscriber = await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId);
                    if (!isValidSubscriber)
                    {
                        return Result.Failure("You are not a valid subscriber");
                    }

                    var WebBaseURL = _configuration["WebBaseURL"];

                    var existingFolder = await _context.Folders.Where(a=>a.Name == request.Name).FirstOrDefaultAsync();
                    if (existingFolder != null)
                    {
                        return Result.Failure("Folder Name already exists");
                    }
                    //check for parentfolderid
                    var parentFolder = await _context.Folders.Where(a => a.Id == request.ParentFolderId).FirstOrDefaultAsync();
                    if(parentFolder == null)
                    {
                        return Result.Failure("Invalid Parent Folder Details");
                    }
                    if(parentFolder.RootFolderId != 0 && parentFolder.RootFolderId != request.RootFolderId)
                    {
                        return Result.Failure("Invalid Root Folder Details");
                    }
                    
                    var folder = new Folder()
                    {
                        Name = request.Name,
                        RootFolderId = request.RootFolderId,
                        ParentFolderId = request.ParentFolderId,
                        FolderPath = $"{WebBaseURL}/{request.RootFolderId}/{request.ParentFolderId}",
                        FolderType = request.FolderType,
                        SubscriberId = request.SubscriberId, 
                        Status = Domain.Enums.Status.Active,
                        StatusDesc = Domain.Enums.Status.Active.ToString(),
                        Description = request.Description,
                        CreatedBy = request.UserId,
                        CreatedByEmail = subscriber.entity.Email,
                        SubscriberName = subscriber.entity.Name,

                    };
                    var folderShareDetails = new List<FolderShareDetail>();
                    if (request.CreateFolderRecipientRequests != null)
                    {
                        foreach (var recipient in request.CreateFolderRecipientRequests)
                        {
                            //if (recipient.RoleId > 0)
                            //{
                                var newRecipient = new FolderShareDetail
                                {
                                    RoleId = recipient.RoleId,
                                    RoleName = recipient.RoleName,
                                    Email = recipient.Email,
                                    FilePermission = recipient.FilePermission,

                                };
                                folderShareDetails.Add(newRecipient);

                            //}
                        }
                    }
                    folder.FolderShareDetails = folderShareDetails;
                    await _context.Folders.AddAsync(folder);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Folder created successfully", folder);

                }
                catch (Exception ex)
                {
                    return Result.Failure($"Folder creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
                }
            }
        }
    }
}
