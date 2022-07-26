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

namespace OnyxDoc.DocumentService.Application.Document.Commands.UpdateDocumentName
{
    public class UpdateDocumentNameCommand : AuthToken, IRequest<Result>
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
    }

    public class UpdateDocumentNameCommandHandler : IRequestHandler<UpdateDocumentNameCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UpdateDocumentNameCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateDocumentNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }

                var document = await _context.Documents.Where(a=>a.Name == request.Name && a.SubscriberId == request.SubscriberId).FirstOrDefaultAsync();
                if (document == null)
                {
                    return Result.Failure(new string[] { "Invalid document details." });
                }
                 _context.Documents.Update(document);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Document name edited successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document edit failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
