using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models; 
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class GetContractRecipientsByContractQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractId { get; set; }
    }


    public class GetContractRecipientsByContractQueryHandler : IRequestHandler<GetContractRecipientsByContractQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractRecipientsByContractQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractRecipientsByContractQuery request, CancellationToken cancellationToken)
        {
            try
            {
                request.OrganisationId = Convert.ToInt32(HttpUtility.UrlEncode(request.OrganisationId.ToString()));
                request.ContractId = Convert.ToInt32(HttpUtility.UrlEncode(request.ContractId.ToString()));

                var entity = await _context.ContractRecipients.Where(a => a.OrganisationId == request.OrganisationId 
                && a.ContractId == request.ContractId ).ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractRecipient), request.ContractId);
                }
                var result = _mapper.Map<List<ContractRecipientDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract recipient by contract id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
