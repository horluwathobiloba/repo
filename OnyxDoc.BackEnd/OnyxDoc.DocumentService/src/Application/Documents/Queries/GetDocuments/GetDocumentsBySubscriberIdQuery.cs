using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Document.Queries.GetDocument;
using OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments;
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
    public class GetDocumentsBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }


    public class GetDocumentsBySubscriberIdQueryHandler : IRequestHandler<GetDocumentsBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDocumentsBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDocumentsBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Documents.Where(a => a.SubscriberId == request.SubscriberId)
                    .ToListAsync();
                if (entity == null || entity.Count() == 0)
                {
                    throw new NotFoundException(nameof(Document));
                }
                var result = _mapper.Map<List<DocumentListDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving document by subscriber id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
