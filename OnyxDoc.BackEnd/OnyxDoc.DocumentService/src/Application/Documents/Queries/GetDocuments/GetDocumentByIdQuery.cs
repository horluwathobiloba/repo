using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Document.Queries.GetDocument;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Document.Queries.GetDocuments
{
    public class GetDocumentByIdQuery : IRequest<Result>
    {
        public int DocumentId { get; set; }

        public string UserId { get; set; }
    }


    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDocumentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Documents.Where(a =>a.Id == request.DocumentId && a.Status == Status.Active)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Document), request.DocumentId);
                }
                var result = _mapper.Map<DocumentListDto>(entity);
                if (result.DocumentStatus == Domain.Enums.DocumentStatus.Expired)
                {
                    return Result.Failure($"This document has expired");
                }
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving document by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
