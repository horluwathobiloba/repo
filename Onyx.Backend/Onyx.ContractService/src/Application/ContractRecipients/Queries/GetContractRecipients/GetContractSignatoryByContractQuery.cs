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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class GetContractSignatoryByContractQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractId { get; set; }
    }


    public class  GetContractSignatoryByContractQueryHandler : IRequestHandler< GetContractSignatoryByContractQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public  GetContractSignatoryByContractQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle( GetContractSignatoryByContractQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractRecipients.Where(a => a.OrganisationId == request.OrganisationId 
                && a.ContractId == request.ContractId && (a.RecipientCategory == RecipientCategory.InternalSignatory.ToString() ||
                    a.RecipientCategory == RecipientCategory.ExternalSignatory.ToString())).ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractRecipient), request.ContractId);
                }
                var signatories = new List<string>();
                foreach (var recipient in entity)
                {
                    signatories.Add(recipient.Email);
                }
               
                return Result.Success(signatories);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract recipient signatories by contract id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
