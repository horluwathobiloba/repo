using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders
{
    public class GetArchivedFolderByIdQuery : AuthToken, IRequest<Result>
    {
        public int FolderId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetArchivedFolderByIdQueryHandler : IRequestHandler<GetArchivedFolderByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetArchivedFolderByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetArchivedFolderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }
                var archivedFolder = await _context.Folders.Where(f => f.Id == request.FolderId && f.SubscriberId == request.SubscriberId && 
                              f.FolderStatus == Domain.Enums.FolderStatus.Archived).Include(f => f.Documents)
                    .Include(f => f.FolderShareDetails)
                    .Include(f => f.Documents)
                    .ThenInclude(f => f.Recipients)
                    .ThenInclude(f => f.RecipientActions).FirstOrDefaultAsync();
                if (archivedFolder == null) return Result.Success($"Archived Folder does not exist - ", archivedFolder);
              

                return Result.Success("Retrieving Archived Folder was successful", archivedFolder);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving archived Folder by id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
