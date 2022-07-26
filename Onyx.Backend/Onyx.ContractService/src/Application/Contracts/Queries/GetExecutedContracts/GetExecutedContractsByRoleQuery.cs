using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetContracts
{
    public class GetExecutedContractsByRoleQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; } 
        public int RoleId { get; set; }
    }


    public class GetExecutedContractsByRoleQueryHandler : IRequestHandler<GetExecutedContractsByRoleQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetExecutedContractsByRoleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetExecutedContractsByRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                
                var entity = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId 
                && a.IsAnExecutedDocument && a.RoleId == request.RoleId)
                    .Include(a => a.Vendor) 
                    .Include(b=> b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Contract), request.RoleId);
                }
                //get all supporting documents in the organisation
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                List<SupportingDocumentDto> documents = new List<SupportingDocumentDto>();
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    documents = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                }
                var result = _mapper.Map<List<ContractDto>>(entity);
                foreach (var executedContract in result)
                {
                    if (documents != null)
                    {
                        executedContract.SupportingDocuments = documents.Where(a => a.ContractId == executedContract.Id).ToList();
                    }
                }
                return Result.Success($"{entity.Count} record(s) found", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract type Initiator by role {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
