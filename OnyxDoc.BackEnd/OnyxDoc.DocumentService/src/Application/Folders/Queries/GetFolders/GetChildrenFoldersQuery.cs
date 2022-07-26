using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders
{
    public class GetChildrenFilesQuery : AuthToken, IRequest<Result>
    {
        public int ParentFolderId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetChildrenFilesQueryHandler : IRequestHandler<GetChildrenFilesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetChildrenFilesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetChildrenFilesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }
                var folders = await _context.Folders.Where(f => f.ParentFolderId == request.ParentFolderId
                                     && f.SubscriberId == request.SubscriberId && f.Status == Domain.Enums.Status.Active && f.FolderStatus != FolderStatus.Archived).Include(f => f.Documents)
                                     .Include(f => f.FolderShareDetails)
                                     .Include(f => f.Documents)
                                     .ThenInclude(f => f.Recipients)
                                     .ThenInclude(f => f.RecipientActions).ToListAsync();

                var document = await _context.Documents.Where(a => a.FolderId == request.ParentFolderId).ToListAsync();

                return Result.Success("Retrieving children files was successful", new { folders = folders , document = document});
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving children files by id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
