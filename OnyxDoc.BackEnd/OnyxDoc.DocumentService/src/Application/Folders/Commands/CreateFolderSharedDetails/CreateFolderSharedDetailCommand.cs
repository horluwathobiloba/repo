using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Queries;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolderSharedDetails
{
    public class CreateFolderSharedDetailCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
        public List<Dictionary<string, FilePermission>> PermissionByEmail { get; set; }
        public List<Dictionary<string, FilePermission>> PermissionByRoleName { get; set; }

    }

    public class CreateFolderSharedDetailCommandHandler : IRequestHandler<CreateFolderSharedDetailCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        public CreateFolderSharedDetailCommandHandler(IMapper mapper, IApplicationDbContext context, IAuthService authService, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(CreateFolderSharedDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var subscriber = await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId);

               
                if (request.PermissionByEmail == null && request.PermissionByRoleName == null || request.PermissionByEmail.Count() == 0 && request.PermissionByRoleName.Count() == 0)
                {
                    return Result.Failure("Permission fields are required");
                }

                var folderSharedDetails = await _context.FolderShareDetails.Where(x => x.FolderId == request.FolderId).ToListAsync(); //pass in foldershareddetails id too
                if(folderSharedDetails.Count() == 0)
                {
                    if (request.PermissionByEmail.Count() > 0 && request.PermissionByRoleName.Count() == 0)
                    {
                        foreach (var email  in request.PermissionByEmail)
                        {
                            var folder = await _context.Folders.Where(x=>x.Id == request.FolderId).FirstOrDefaultAsync();
                            if (folder == null) return Result.Failure("Folder not existing");
                            foreach (var detail in folder.FolderShareDetails)
                            {
                                var sharedFolderDetail = new FolderShareDetail
                                {
                                    FolderId = request.FolderId,
                                    Email = email.Keys.ToString(),
                                    CreatedBy = subscriber.entity.ContactEmail
                                };
                                _context.FolderShareDetails.Add(sharedFolderDetail);

                            }
                            
                        }

                    }
                    else
                    {
                        foreach (var roleName in request.PermissionByRoleName)
                        {
                            var sharedFolderDetail = new FolderShareDetail
                            {
                                FolderId= request.FolderId,
                                RoleName = roleName.Keys.ToString(),
                                CreatedBy = subscriber.entity.ContactEmail,
                            };
                            _context.FolderShareDetails.Add(sharedFolderDetail);
                        }
                    }
                }

                if (request.PermissionByEmail.Count() > 0 && request.PermissionByRoleName.Count() == 0)
                {
                    foreach (var item in request.PermissionByEmail)
                    {
                        //item.Email = 
                        foreach (var existingFolderDetail in folderSharedDetails)
                        {
                            //var sharedFolderDetailEntity = _mapper.Map<FolderShareDetail>(item.Values);
                            if (item.ContainsKey(existingFolderDetail.Email))
                            {
                                existingFolderDetail.FilePermission = item[existingFolderDetail.Email];
                                existingFolderDetail.LastModifiedBy = subscriber.entity.CreatedBy;
                                _context.FolderShareDetails.Update(existingFolderDetail);
                            }
                            else
                            {
                                var sharedFolderDetail = new FolderShareDetail
                                {
                                    Email = item.Keys.ToString(),
                                    CreatedBy= subscriber.entity.ContactEmail
                                };
                                _context.FolderShareDetails.Add(sharedFolderDetail);
                            }

                        }
                    }
                }


                if (request.PermissionByEmail.Count() == 0 && request.PermissionByRoleName.Count() > 0)
                {
                    foreach (var item in request.PermissionByEmail)
                    {
                        //item.Email = 
                        foreach (var existingFolderDetail in folderSharedDetails)
                        {
                            //var sharedFolderDetailEntity = _mapper.Map<FolderShareDetailDto>(item.Values);
                            if (item.ContainsKey(existingFolderDetail.RoleName))
                            {
                                existingFolderDetail.FilePermission = item[existingFolderDetail.RoleName];
                                existingFolderDetail.LastModifiedBy = subscriber.entity.CreatedBy;
                                _context.FolderShareDetails.Update(existingFolderDetail);
                            }
                            else
                            {
                                var sharedFolderDetail = new FolderShareDetail
                                {
                                    RoleName = item.Keys.ToString(),
                                    CreatedBy = subscriber.entity.ContactEmail
                                };
                                _context.FolderShareDetails.Add(sharedFolderDetail);
                            }


                        }
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Folder shared details successfully created!");

            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder shared detail creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
