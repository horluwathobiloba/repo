using MediatR;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateRootFolder
{
    public class CreateRootFolderCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int RootFolderId { get; set; }
        public int ParentFolderId { get; set; }
        public string UserId { get; set; }
    }

    public class CreateRootFolderCommandHandler : IRequestHandler<CreateRootFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        public CreateRootFolderCommandHandler(IApplicationDbContext context, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(CreateRootFolderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                var subscriber = await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var WebBaseURL = _configuration["WebBaseURL"];
                if (request.RootFolderId != 0 && request.ParentFolderId != 0)
                {
                    return Result.Failure("Invalid Root Folder Creation Details");
                }
                var folders = new List<Folder>();
                foreach (FolderType folderType in Enum.GetValues(typeof(FolderType)))
                {
                    var folder = new Folder()
                    {
                        Name = folderType.ToString(),
                        RootFolderId = request.RootFolderId,
                        ParentFolderId = request.ParentFolderId,
                        FolderPath = $"{WebBaseURL}/{request.RootFolderId}/{request.ParentFolderId}",
                        FolderType = folderType,
                        SubscriberId = request.SubscriberId,
                        Status = Domain.Enums.Status.Active,
                        StatusDesc = Domain.Enums.Status.Active.ToString(),
                        Description = "Root folder - " + folderType.ToString(),
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = subscriber.entity.Email,
                        SubscriberName = subscriber.entity.Name,
                    };

                    folders.Add(folder);
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
