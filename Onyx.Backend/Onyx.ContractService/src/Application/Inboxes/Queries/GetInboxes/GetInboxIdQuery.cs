using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Inboxes.Queries.GetInboxes
{
    public class GetInboxByIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }

    public class GetInboxByIdQueryHandler : IRequestHandler<GetInboxByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetInboxByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetInboxByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                } 
                var Inbox = await _context.Inboxes.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);

                if (Inbox == null)
                {
                    throw new NotFoundException(nameof(Inbox));
                }
                var result = _mapper.Map<InboxDto>(Inbox);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving vendor type. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
