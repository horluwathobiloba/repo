using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder;
using OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder.CreateFolderCommand;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.DuplicateFolder
{
    public class DuplicateFolderCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class DuplicateFolderCommandHndler : IRequestHandler<DuplicateFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public DuplicateFolderCommandHndler(IApplicationDbContext context, IAuthService authService, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<Result> Handle(DuplicateFolderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var folder = await _context.Folders.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
                if (folder == null)
                {
                    return Result.Failure("Invalid Folder Id");
                }
                var folderCommand = new CreateFolderCommand();
                var folderHandler = new CreateFolderCommandHandler(_context, _authService, _configuration);
                var createFolderCommand = new CreateFolderCommand
                {
                    AccessToken = request.AccessToken,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    CreateFolderRecipientRequests = null,
                    Description = folder.Description,
                    FolderType = folder.FolderType,
                    Name = "Copy of " + folder.Name ,
                    ParentFolderId = folder.ParentFolderId,
                    RootFolderId = folder.RootFolderId,
                };
                var folderCreation = await folderHandler.Handle(createFolderCommand, cancellationToken);
                if (!folderCreation.Succeeded)
                {
                    return Result.Failure($"Error creating duplicate folder {folderCreation.Error}");
                }

                folder.IsDuplicated = true;

                _context.Folders.Update(folder);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success($"{createFolderCommand.Name} folder has been duplicated");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to duplicate folder. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
