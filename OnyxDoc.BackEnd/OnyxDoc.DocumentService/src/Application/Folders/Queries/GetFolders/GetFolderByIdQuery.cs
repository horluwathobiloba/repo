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
    public class GetFolderByIdQuery : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetFolderByIdQueryHandler : IRequestHandler<GetFolderByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetFolderByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetFolderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }
                var folder = await _context.Folders.Where(f => f.Id == request.Id && f.SubscriberId == request.SubscriberId 
                              && f.Status == Domain.Enums.Status.Active).Include(f => f.Documents)
                              .Include(f => f.FolderShareDetails)
                              .Include(f => f.Documents)
                              .ThenInclude(f => f.Recipients)
                              .ThenInclude(f => f.RecipientActions).FirstOrDefaultAsync();
                if (folder == null) return Result.Success($"No Folder exists with this id {request.Id}");
                
                return Result.Success("Folder fetch successful", folder);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving folder by id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
