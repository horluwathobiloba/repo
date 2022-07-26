using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxesByEmail
{
    public class GetInboxesByEmailQuery : IRequest<Result>
    {
        public string Email { get; set; }
    }


    public class GetInboxesByEmailQueryHandler : IRequestHandler<GetInboxesByEmailQuery, Result>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetInboxesByEmailQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetInboxesByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //we need to figure out how to handle fpr recieved
                
                var list = await _context.Inboxes.Where(a =>request.Email == a.Email)
                    .Include(x=>x.Document)
                    .ThenInclude(x=>x.Recipients)
                    .ThenInclude(x=>x.RecipientActions)
                    .ToListAsync();
               

                //var reci

                if (list == null)
                {
                    throw new NotFoundException(nameof(Inbox));
                }
                return Result.Success(list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"License type status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
