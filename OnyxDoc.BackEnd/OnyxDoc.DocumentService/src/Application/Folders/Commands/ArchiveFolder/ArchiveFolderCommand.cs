using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.ArchiveFolder
{
    public class ArchiveFolderCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }
    public class ArchiveFolderCommandHandler : IRequestHandler<ArchiveFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public readonly IMapper _mapper;
        public ArchiveFolderCommandHandler(IApplicationDbContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ArchiveFolderCommand request, CancellationToken cancellationToken)
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
                folder.FolderStatus = Domain.Enums.FolderStatus.Archived;
                folder.FolderStatusDesc = Domain.Enums.FolderStatus.Archived.ToString();
                _context.Folders.Update(folder);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success($"{folder.Name} folder has been archived successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder archive failed. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
