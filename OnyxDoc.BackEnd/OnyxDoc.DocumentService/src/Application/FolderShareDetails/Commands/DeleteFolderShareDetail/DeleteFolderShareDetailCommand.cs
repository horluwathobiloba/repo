using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.FolderShareDetails.Commands.DeleteFolderShareDetail
{
    public class DeleteFolderShareDetailCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteFolderShareDetailCommandHandler : IRequestHandler<DeleteFolderShareDetailCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public readonly IMapper _mapper;
        public DeleteFolderShareDetailCommandHandler(IApplicationDbContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteFolderShareDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isValidSubscriber = await _authService.IsValidSubscriber(request.AccessToken, request.SubscriberId, request.UserId);
                if (!isValidSubscriber)
                {
                    return Result.Failure("You are not a valid subscriber");
                }

                var folderShareDetail = _context.FolderShareDetails.Where(f => f.Id == request.Id).FirstOrDefaultAsync();
                if (folderShareDetail == null)
                {
                    return Result.Failure("Invalid Folder Id");
                }

                 _context.FolderShareDetails.Remove(folderShareDetail.Result);
                await _context.SaveChangesAsync(cancellationToken);
               

                return Result.Success($"Recipient {folderShareDetail.Result.Email} has been deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder shared detail delete failed. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
