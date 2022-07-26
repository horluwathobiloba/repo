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
    public class GetExecutedContractsQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetExecutedContractsQueryHandler : IRequestHandler<GetExecutedContractsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetExecutedContractsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetExecutedContractsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.IsAnExecutedDocument)
                    ?.Include(a => a.Vendor)
                    ?.Include(b => b.ProductServiceType)
                    ?.Include(b => b.PaymentPlan)
                    ?.Include(b => b.ContractDuration)
                    ?.ToListAsync();
                ////this is because when we add include for objects that are null, it doesnt return any value
                //var saveAsDraftExecutedContracts = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.IsAnExecutedDocument
                //  && (a.VendorId == 0 || a.ProductServiceTypeId == 0 || a.PaymentPlanId == 0 || a.ContractDurationId == null))
                // .ToListAsync();
                //list.AddRange(saveAsDraftExecutedContracts);
                //list.OrderBy(a=>a.CreatedDate);
                //get all supporting documents in the organisation
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();

                List<SupportingDocumentDto> documents = new List<SupportingDocumentDto>();
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    documents = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                }
                if (list == null)
                {
                    throw new NotFoundException(nameof(Contract));
                }
                var result = _mapper.Map<List<ContractDto>>(list);

                foreach (var executedContract in result)
                {
                    if (documents != null)
                    {
                        executedContract.SupportingDocuments = documents.Where(a => a.ContractId == executedContract.Id).ToList(); 
                    }
                }
               
                return Result.Success($"{list.Count} record(s) found", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving executed contract. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
