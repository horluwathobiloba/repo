using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateParentFolder;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateRootFolder;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder.CreateFoldersCommand;

namespace OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders
{
    public class GetFoldersByUserIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string AuthToken { get; set; }
        public string UserId { get; set; }
    }
    public class GetFoldersByUserIdQueryHandler : IRequestHandler<GetFoldersByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public GetFoldersByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(GetFoldersByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AuthToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var folderList = await _context.Folders.Where(x => x.SubscriberId == request.SubscriberId && x.CreatedBy == request.UserId
                            && x.Status == Status.Active && x.FolderStatus != FolderStatus.Archived)
                       .Include(f => f.FolderShareDetails)
                       .Include(f => f.Documents)
                       .ThenInclude(f => f.Recipients)
                       .ThenInclude(f => f.RecipientActions)
                       .ToDictionaryAsync(x => x.Id);

                //Create Default Folders
                if (folderList == null || folderList.Count() == 0)
                {
                    var rootFolderCommand = new CreateRootFolderCommand();
                    var folderListHandler = new CreateRootFolderCommandHandler(_context, _authService, _configuration);
                    var createRootFoldersCommand = new CreateRootFolderCommand
                    {
                        AccessToken = request.AuthToken,
                        SubscriberId = request.SubscriberId,
                        UserId = request.UserId,
                       
                    };
                    var folderCreation = await folderListHandler.Handle(createRootFoldersCommand, cancellationToken);
                    if (!folderCreation.Succeeded)
                    {
                        return Result.Failure("Please contact support! Error creating default folders");
                    }
                    folderList = await _context.Folders.Where(x => x.SubscriberId == request.SubscriberId && x.CreatedBy == request.UserId 
                                 && x.Status == Status.Active && x.FolderStatus != FolderStatus.Archived)
                       .Include(f=>f.FolderShareDetails)
                       .Include(f => f.Documents)
                       .ThenInclude(f => f.Recipients)
                       .ThenInclude(f => f.RecipientActions)
                       .ToDictionaryAsync(x => x.Id);
                }

               
                var parentFolders = folderList.Where(x => x.Value.RootFolderId == 0 && x.Value.ParentFolderId == 0);
                if (parentFolders == null || parentFolders.Count() == 0)
                {
                    return Result.Failure("No available folders! ParentFolder is -" + parentFolders);
                }
                
                var directParentFolderList = folderList.Where(x => x.Value.RootFolderId >= 0 && x.Value.ParentFolderId >= 0);//check
                if (directParentFolderList == null || directParentFolderList.Count() == 0)
                {
                    return Result.Failure("Please create a folder!");
                }
              
                var result = new Dictionary<FolderType, List<object>>();
                var myFolderList = new List<object>();
                var myOrgFolderList = new List<object>();
                var sharedWithMeFolderList = new List<object>();
                var sharedWithOthersFolderList = new List<object>();
                var publicFoldersList = new List<object>();
                //var folderType = GetAllFoldersResult((int)FolderType.MyFolder, folderList, directParentFolderList, folderDictionary, parentFolder)).
                var folderDictionary = new Dictionary<int, List<Folder>>();
                foreach (var parentFolder in parentFolders)
                {
                    switch (parentFolder.Value.FolderType)
                    {
                        case FolderType.MyFolder:
                            myFolderList.Add(GetAllFoldersResult(parentFolder.Value.FolderType, folderList, directParentFolderList, parentFolder));
                            break;
                        case FolderType.MyOrganization:
                            myOrgFolderList.Add(GetAllFoldersResult(parentFolder.Value.FolderType, folderList, directParentFolderList, parentFolder));
                            break;
                        case FolderType.SharedWithMe:
                            sharedWithMeFolderList.Add(GetAllFoldersResult(parentFolder.Value.FolderType, folderList, directParentFolderList, parentFolder));
                            break;
                        case FolderType.SharedWithOthers:
                            sharedWithOthersFolderList.Add(GetAllFoldersResult(parentFolder.Value.FolderType, folderList, directParentFolderList, parentFolder));
                            break;
                        case FolderType.Public:
                            publicFoldersList.Add(GetAllFoldersResult(parentFolder.Value.FolderType, folderList, directParentFolderList, parentFolder));
                            break;
                        default:
                            break;
                    }
                }
                result.Add(FolderType.MyFolder, myFolderList);
                result.Add(FolderType.MyOrganization, myOrgFolderList);
                result.Add(FolderType.SharedWithMe, sharedWithMeFolderList);
                result.Add(FolderType.SharedWithOthers, sharedWithOthersFolderList);
                result.Add(FolderType.Public, publicFoldersList);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving folders {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
        private object GetAllFoldersResult(FolderType folderType, Dictionary<int, Folder> folderList, IEnumerable<KeyValuePair<int, Folder>> directParentFolderList,
                                         KeyValuePair<int, Folder> parentFolder)
        {
           
            
            var idsOfMyChildrenFolder = new List<int>();
            var myChildrenFolders = new List<Folder>();
             myChildrenFolders = GetChildFolders(folderList, parentFolder.Value, directParentFolderList)?
                                                  .Where(x => x.FolderType == folderType && x.ParentFolderId > 0)?
                                                  .ToList();
            if (myChildrenFolders == null || myChildrenFolders.Count() == 0)
            {
                return new {
                    Parent = parentFolder,
                    Children = myChildrenFolders, 
                    DirectParentIds = idsOfMyChildrenFolder?.ToArray()
                };
            }
            var childFolderStatus = myChildrenFolders.FirstOrDefault().Status;
            foreach (var childrenFolder in myChildrenFolders)
            {
                idsOfMyChildrenFolder.Add(childrenFolder.Id);
            }
            var result = new { Parent = parentFolder, Children = myChildrenFolders, DirectParentIds = idsOfMyChildrenFolder.ToArray() };
            return result;
        }

      
        private List<Folder> GetChildFolders(Dictionary<int, Folder> folderList, Folder parentFolder, IEnumerable<KeyValuePair<int, Folder>> directParentFolderList)
        {
            //var folderDictionary = new Dictionary<Folder, List<Folder>>();
            var childrenFolders = new List<Folder>();
            foreach (var folderId in directParentFolderList)
            {
                if (folderList.ContainsKey(folderId.Key))
                {
                    var folder = folderList[folderId.Key];
                    if (folder.RootFolderId == parentFolder.Id)
                    {
                        childrenFolders.Add(folderList[folderId.Key]);
                    }
                }
            }
            return childrenFolders;
        }
        
    }

}