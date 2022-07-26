using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OnyxDoc.DocumentService.Domain.Constants;

namespace OnyxDoc.DocumentService.Application.Document.Commands.DownloadDocument
{
    public class DownloadDocumentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class DownloadDocumentCommandHandler : IRequestHandler<DownloadDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public DownloadDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(a=>a.SubscriberId == request.SubscriberId && a.UserId == request.UserId);
                if (document == null)
                {
                    return Result.Failure("User has no valid document!");
                }
                return Result.Success(document.File);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Document download failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
    