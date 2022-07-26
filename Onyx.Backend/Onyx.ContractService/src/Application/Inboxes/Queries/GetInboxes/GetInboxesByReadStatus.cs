using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Inboxes.Queries.GetInboxes
{
    public class GetInboxesByReadStatus:AuthToken,IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string Email { get; set; }
    }

    public class GetInboxesByReadStatusQueryHandler : IRequestHandler<GetInboxesByReadStatus, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetInboxesByReadStatusQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetInboxesByReadStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                if (request.OrganizationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                var inboxUnread = await _context.Inboxes.Where(a => a.OrganisationId == request.OrganizationId && a.Read ==false&&a.ReciepientEmail==request.Email).ToListAsync();
                var inboxRead = await _context.Inboxes.Where(a => a.OrganisationId == request.OrganizationId && a.Read ==true&&a.ReciepientEmail==request.Email).ToListAsync();


                var result = new
                {
                    inboxRead = inboxRead,
                    inboxUnread = inboxUnread
                };
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving data type. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
