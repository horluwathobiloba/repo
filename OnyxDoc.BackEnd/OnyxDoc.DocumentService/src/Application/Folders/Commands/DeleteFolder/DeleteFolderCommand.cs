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

namespace OnyxDoc.DocumentService.Application.Folders.Commands.DeleteFolder
{
    public class DeleteFolderCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }
    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public readonly IMapper _mapper;
        public DeleteFolderCommandHandler(IApplicationDbContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
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
                folder.FolderStatus = Domain.Enums.FolderStatus.Deleted;
                folder.FolderStatusDesc = Domain.Enums.FolderStatus.Deleted.ToString();
                folder.Status = Domain.Enums.Status.Inactive;
                folder.StatusDesc = Domain.Enums.Status.Inactive.ToString();

                _context.Folders.Update(folder);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success($"{folder.Name} folder has been deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder delete failed. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
