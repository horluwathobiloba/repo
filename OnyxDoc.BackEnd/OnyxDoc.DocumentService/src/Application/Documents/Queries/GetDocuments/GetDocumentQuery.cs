using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments
{
    public class GetDocumentQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }


    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDocumentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Documents
                    .ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Document));
                }
                var result = _mapper.Map<DocumentDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving document by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
