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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients
{
    public class GetRecipientsQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
    }

    public class GetRecipientsQueryHandler : IRequestHandler<GetRecipientsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipientsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRecipientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _context.Recipients.Where(a => a.SubscriberId == request.SubscriberId )
                    .ToListAsync();

                if (list == null)
                {
                    throw new NotFoundException(nameof(Recipient));
                }
                var result = _mapper.Map<List<RecipientDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving  type Initiators. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
