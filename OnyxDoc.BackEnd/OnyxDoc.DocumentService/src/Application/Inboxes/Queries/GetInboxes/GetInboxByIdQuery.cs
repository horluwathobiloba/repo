using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxesByEmail
{
    public class GetInboxByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
    }

    public class GetInboxesByIdQueryHandler : IRequestHandler<GetInboxByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetInboxesByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetInboxByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var inboxes = await _context.Inboxes.Where(a => a.Id == request.Id && a.DocumentId == request.DocumentId)
                    .Include(x => x.Document).ThenInclude(x => x.Recipients).ThenInclude(x => x.RecipientActions)
                    .ToListAsync();
                //.Include(x => x.Document)
                //.Include(x => x.Recipient).ToListAsync();
               
                
                var result = new List<Domain.Entities.Inbox>(); 

                if (result == null)
                {
                    throw new NotFoundException(nameof(result));
                }

                foreach (var inbox in inboxes)
                {
                    result.Add(inbox);
                }
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving inbox. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
