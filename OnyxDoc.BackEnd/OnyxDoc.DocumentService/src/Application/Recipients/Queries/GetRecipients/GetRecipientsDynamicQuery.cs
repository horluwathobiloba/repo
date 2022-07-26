using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients
{
    public class GetRecipientsDynamicQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetRecipientsDynamicQueryHandler : IRequestHandler<GetRecipientsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRecipientsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRecipientsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = (await _context.Recipients
                    .Where(a => a.SubscriberId == request.SubscriberId)
                    .ToListAsync())
                    .Where(a => request.SearchText.IsIn(a.Email));

                if (list == null)
                {
                    throw new NotFoundException(nameof(Recipient));
                }
                var result = _mapper.Map<List<RecipientDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving workflow phases. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
