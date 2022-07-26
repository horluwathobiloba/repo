using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipient.Queries.GetRecipient
{
    public class GetRecipientsByDocumentIdQuery : IRequest<Result>
    {
        public int DocumentId { get; set; }

        public string UserId { get; set; }
    }


    public class GetRecipientsByDocumentIdQueryHandler : IRequestHandler<GetRecipientsByDocumentIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipientsByDocumentIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRecipientsByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Recipients.Where(a =>  a.DocumentId == request.DocumentId)
                    .ToListAsync();
                if (entity == null || entity.Count() == 0)
                {
                    throw new NotFoundException(nameof(Recipient), request.DocumentId);
                }
                var result = _mapper.Map<List<RecipientListDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving recipient by document id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
