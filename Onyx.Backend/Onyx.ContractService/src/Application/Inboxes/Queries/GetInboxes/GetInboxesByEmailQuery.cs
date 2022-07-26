using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Inboxes.Queries.GetInboxes
{
    public class GetInboxesByNameQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string Email { get; set; }
    }
    public class GetInboxesByNameQueryHandler : IRequestHandler<GetInboxesByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetInboxesByNameQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetInboxesByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return Result.Failure($"Email must be specified.");
                }

                var list = await _context.Inboxes.Where(a => a.OrganisationId == request.OrganisationId
                    && request.Email.ToLower() == a.ReciepientEmail.ToLower()||request.Email==a.CreatedByEmail).ToListAsync();

                if (list == null)
                {
                    throw new NotFoundException(nameof(Inbox));
                }
                var result = _mapper.Map<List<InboxDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving inboxes. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
