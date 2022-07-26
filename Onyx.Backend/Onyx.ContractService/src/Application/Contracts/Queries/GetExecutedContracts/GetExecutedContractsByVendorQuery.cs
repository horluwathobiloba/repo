using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetContracts
{
    public class GetExecutedContractsByVendorQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int VendorId { get; set; }
    }


    public class GetExecutedContractsByVendorQueryHandler : IRequestHandler<GetExecutedContractsByVendorQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetExecutedContractsByVendorQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetExecutedContractsByVendorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId 
                &&  a.IsAnExecutedDocument && a.VendorId == request.VendorId)
                    .Include(a => a.Vendor)
                    .Include(b => b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .ToListAsync();
                //get all supporting documents
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                List<SupportingDocumentDto> documents = new List<SupportingDocumentDto>();
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    documents = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                }
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Contract), request.VendorId);
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
                return Result.Failure($"Error retrieving contract by vendor. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
