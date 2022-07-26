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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.DocumentService.Application.Commands.DecodeUrlHash
{
    public class DecodeUrlHashCommand : IRequest<Result>
    {
        public string DocumentLinkHash { get; set; }
    }

    public class DecodeUrlHashCommandHandler : IRequestHandler<DecodeUrlHashCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;

        public DecodeUrlHashCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IStringHashingService stringHashingService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
        }
        public async Task<Result> Handle(DecodeUrlHashCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.DocumentLinkHash = HttpUtility.UrlEncode(request.DocumentLinkHash);
                List<Domain.Entities.Component> components = await _context.Components.Include(a=>a.Coordinate).Where(x => x.Hash == request.DocumentLinkHash && x.Status == Status.Active).ToListAsync();
                if (components == null || components.Count == 0)
                {
                    return Result.Failure($"No components exists with this Hash");
                }
                var firstComponent = components.FirstOrDefault();
                var rank = firstComponent.Rank;
                var documentId = firstComponent.DocumentId;
                var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

                var lastRecipientAction = await _context.RecipientActions.Where(a => a.DocumentId == documentId &&
                                                                          a.AppSigningUrl.Contains(request.DocumentLinkHash)).FirstOrDefaultAsync();
                if (lastRecipientAction != null)
                {
                    return Result.Failure($"This document has been actioned already");
                }

                if (document.DocumentStatus == Domain.Enums.DocumentStatus.Expired)
                {
                    return Result.Failure($"This document has expired");
                }
                var maxRank = await _context.Recipients.Where(a=>a.DocumentId == documentId).MaxAsync(a => a.Rank);
                var previousComponents = await _context.Components.Include(a => a.Coordinate).Where(a => a.Rank < firstComponent.Rank && a.DocumentId == documentId && a.Status == Status.Active).ToListAsync();
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(new 
                { 
                    ActiveRank=rank, 
                    Component = components,
                    PreviousComponents = previousComponents ,
                    IsLastRank = (maxRank == rank) ?true : false
                } 
                );
            }
            catch (Exception ex)
            {
                return Result.Failure($"Component retrieval failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
