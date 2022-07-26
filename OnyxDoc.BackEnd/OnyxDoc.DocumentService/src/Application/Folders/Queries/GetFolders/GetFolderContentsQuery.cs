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
    public class GetFolderContentsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RootFolderId { get; set; }
        public FolderType FolderType { get; set; }

    }

    public class GetFolderContentsQueryHandler : IRequestHandler<GetFolderContentsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetFolderContentsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetFolderContentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }

                var childFolders = await _context.Folders
                    .Where(f => f.RootFolderId == request.RootFolderId && f.SubscriberId == request.SubscriberId && f.Status == Domain.Enums.Status.Active && f.FolderType == request.FolderType)
                    .Include(f => f.FolderShareDetails)
                    .Include(f=>f.Documents)
                    .ThenInclude(f=>f.Recipients)
                    .ThenInclude(f=>f.RecipientActions)
                    .ToListAsync();
                if (childFolders == null || childFolders.Count() == 0) return Result.Success($"Root folder is empty!", childFolders);
                

                return Result.Success("Retrieving child folders successful", childFolders);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving child folders by id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
