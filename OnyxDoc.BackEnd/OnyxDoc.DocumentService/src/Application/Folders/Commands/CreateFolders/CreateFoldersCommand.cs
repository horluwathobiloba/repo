using MediatR;
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
    public class CreateFoldersCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }

        public string UserId { get; set; }
        public List<CreateFolderListRequest> CreateFolderListRequest { get; set; }



        public class CreateFoldersCommandHandler : IRequestHandler<CreateFoldersCommand, Result>
        {
            private readonly IApplicationDbContext _context;
            private readonly IAuthService _authService;
            private readonly IConfiguration _configuration;

            public CreateFoldersCommandHandler(IApplicationDbContext context, IAuthService authService, IConfiguration configuration)
            {
                _context = context;
                _authService = authService;
                _configuration = configuration;
            }
            public async Task<Result> Handle(CreateFoldersCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                    var subscriber = await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId);

                    var WebBaseURL = _configuration["WebBaseURL"];


                    if (!isValidSubscriber)
                    {
                        return Result.Failure("You are not a valid subscriber");
                    }
                    List<Folder> folders = new List<Folder>();
                    List<FolderShareDetail> folderShareDetails = new List<FolderShareDetail>();
                    foreach (var folder in request.CreateFolderListRequest)
                    {
                        var folderForCreation = new Folder()
                        {
                            Name = folder.Name,
                            ParentFolderId = folder.ParentFolderId,
                            RootFolderId = folder.RootFolderId,
                            FolderPath = $"{WebBaseURL}/{folder.RootFolderId}/{folder.ParentFolderId}",
                            FolderType = folder.FolderType,
                            SubscriberId = request.SubscriberId,
                            Status = Domain.Enums.Status.Active,
                            StatusDesc = Domain.Enums.Status.Active.ToString(),
                            Description = folder.Description,
                            CreatedBy = request.UserId,
                            CreatedByEmail = subscriber.entity.Email,
                            SubscriberName = subscriber.entity.Name,
                            
                    };

                        if (folder.CreateFolderRecipientRequests != null || folder.CreateFolderRecipientRequests.Count() > 0)
                        {
                            foreach (var recipient in folder.CreateFolderRecipientRequests)
                            {
                                var folderShareDetail = new FolderShareDetail
                                {
                                    RoleId = recipient.RoleId,
                                    RoleName = recipient.RoleName,
                                    Email = recipient.Email,
                                    FilePermission = recipient.FilePermission
                                };
                                folderShareDetails.Add(folderShareDetail);
                            }
                            folderForCreation.FolderShareDetails = folderShareDetails;
                        }
                        folders.Add(folderForCreation);
                    }

                    await _context.Folders.AddRangeAsync(folders);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Folders created successfully", folders);

                }
                catch (Exception ex)
                {
                    return Result.Failure($"Folders creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
                }
            }
        }
    }
}
