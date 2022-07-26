using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class GetContractRecipientsQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetContractRecipientsQueryHandler : IRequestHandler<GetContractRecipientsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractRecipientsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractRecipientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _context.ContractRecipients.Where(a => a.OrganisationId == request.OrganisationId )
                    .Include(a => a.Contract) 
                    .ToListAsync();

                if (list == null)
                {
                    throw new NotFoundException(nameof(ContractRecipient));
                }
                var result = _mapper.Map<List<ContractRecipientDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract type Initiators. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
