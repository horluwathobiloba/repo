using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.MoveFolder
{
    public class MoveFolderCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int ParentFolderId { get; set; }
        public int RootFolderId { get; set; }
    }

    public class MoveFolderCommandHandler : IRequestHandler<MoveFolderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public MoveFolderCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(MoveFolderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var WebBaseURL = _configuration["WebBaseURL"];
                var command = new GetFolderByIdQuery()
                {
                    Id = request.Id,
                    SubscriberId = request.SubscriberId,
                    UserId =request.UserId,
                    AccessToken = request.AccessToken

                };

                var folder = await _context.Folders.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
                if (folder == null)
                {
                    return Result.Failure("Invalid Folder Id");
                }
                folder.ParentFolderId = request.ParentFolderId;
                folder.RootFolderId = request.RootFolderId;
                folder.FolderPath = $"{WebBaseURL}/{request.ParentFolderId}/{request.RootFolderId}";

                _context.Folders.Update(folder);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Folder moved successfully",  folder);


                //var folder = _mapper.Map<Folder>(folderEntity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Folder edit failed. Error:{ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
