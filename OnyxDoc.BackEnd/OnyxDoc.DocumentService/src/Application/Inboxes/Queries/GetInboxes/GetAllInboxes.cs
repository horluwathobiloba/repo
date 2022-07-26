using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxes
{
    public class GetAllInboxes : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int DocumentId { get; set; }
    }

    public class GetAllInboxesHandler : IRequestHandler<GetAllInboxes, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetAllInboxesHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetAllInboxes request, CancellationToken cancellationToken)
        {
            try
            {
                var inboxes = await _context.Inboxes.Where(x => x.DocumentId == request.DocumentId && x.SubscriberId == request.SubscriberId)
                    .Include(x=>x.Document)
                    .ThenInclude(x=>x.Recipients)
                    .ThenInclude(x=>x.RecipientActions)
                    .ToListAsync();
                if (inboxes == null)
                {
                    return Result.Failure("You have no inbox yet");
                }

                return Result.Success(inboxes);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving inbox. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }

}
