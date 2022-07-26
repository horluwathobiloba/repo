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
    public class GetArchivedFoldersQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetArchivedFoldersQueryHandler : IRequestHandler<GetArchivedFoldersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetArchivedFoldersQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetArchivedFoldersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }
                var archivedFolders = await _context.Folders.Where(f =>  f.SubscriberId == request.SubscriberId && 
                              f.FolderStatus == Domain.Enums.FolderStatus.Archived).Include(f => f.Documents)
                    .Include(f => f.FolderShareDetails)
                    .Include(f => f.Documents)
                    .ThenInclude(f => f.Recipients)
                    .ThenInclude(f => f.RecipientActions).ToListAsync();
                if (archivedFolders == null || archivedFolders.Count() == 0) return Result.Success($"Archived Folders does not exist - ", archivedFolders);
              

                return Result.Success("Retrieving Archived Folders was successful", archivedFolders);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving archived Folders by id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
