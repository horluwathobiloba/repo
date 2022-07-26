using AutoMapper;
using MediatR;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.EditFolder
{
    public class EditFolderCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public List<EditFolderRecipientRequest> EditFolderRecipientRequests { get; set; }
    }

    public class EditFolderCommandHandler : IRequestHandler<EditFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public EditFolderCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(EditFolderCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);

                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var subscriber = await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId);
                var command = new GetFolderByIdQuery()
                {
                    Id = request.Id,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                //Check on folder recipient if he has the right to edit
                //Created by Id can do anything on the folder

                var handler = new GetFolderByIdQueryHandler(_context, _mapper, _authService);
                var result = handler.Handle(command, cancellationToken);
                var folderEntity = result.Result.Entity;
                if (folderEntity == null)
                {
                    return Result.Failure("Folder not existing");
                }

                var folder = _mapper.Map<Folder>(folderEntity);
                folder.Name = request.Name;
                folder.LastModifiedBy = subscriber.entity.Email;
                
                var folderRecipients = new Dictionary<int?, FolderShareDetail>();
                foreach (var sharedFolderDetail in request.EditFolderRecipientRequests)
                {
                    //var sharedFolderDetailEntity = _mapper.Map<FolderShareDetail>(sharedFolderDetail);
                    var sharedFolderDetailEntity = new FolderShareDetail
                    {
                        Id = sharedFolderDetail.Id,
                        RoleId = sharedFolderDetail.RoleId,
                        RoleName = sharedFolderDetail.RoleName,
                        SharedWithName = sharedFolderDetail.SharedWithName,
                        Email = sharedFolderDetail.Email,
                        FilePermission = sharedFolderDetail.FilePermission,
                    };

                    if (sharedFolderDetail.Id == 0)
                    {
                       await _context.FolderShareDetails.AddAsync(sharedFolderDetailEntity);
                    }
                    folderRecipients.Add(sharedFolderDetail.Id, sharedFolderDetailEntity);

                }
                foreach (var sharedFolderDetail in folder.FolderShareDetails)
                {
                    if (folderRecipients.ContainsKey(sharedFolderDetail.Id))
                    {
                        sharedFolderDetail.RoleId = folderRecipients[sharedFolderDetail.Id].RoleId;
                        sharedFolderDetail.RoleName = folderRecipients[sharedFolderDetail.Id].RoleName;
                        sharedFolderDetail.SharedWithName = folderRecipients[sharedFolderDetail.Id].SharedWithName;
                        sharedFolderDetail.Email = folderRecipients[sharedFolderDetail.Id].Email;
                        sharedFolderDetail.FilePermission = folderRecipients[sharedFolderDetail.Id].FilePermission;
                        _context.FolderShareDetails.Update(sharedFolderDetail);
                    }
                }

                _context.Folders.Update(folder);
                await _context.SaveChangesAsync(cancellationToken);

                //Get by Id
                //Update

                return Result.Success("Folder name edited successfully!", folder);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder edit failed. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
