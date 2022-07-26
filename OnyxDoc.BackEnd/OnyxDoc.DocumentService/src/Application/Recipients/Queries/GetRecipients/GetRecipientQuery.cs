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

namespace OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients
{
    public class GetRecipientQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
    }


    public class GetRecipientQueryHandler : IRequestHandler<GetRecipientQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRecipientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Recipients.Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Recipient), request.Id);
                }
                var result = _mapper.Map<RecipientDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving  recipient by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
