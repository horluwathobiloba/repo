using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : AuthToken, IRequest<Result>
    {
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public  DeleteDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.UserId == request.UserId && a.Id == request.DocumentId);
                if (document == null)
                {
                    return Result.Failure("User has no valid document!");
                }
                return Result.Success(document.File);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Document Deletion failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
