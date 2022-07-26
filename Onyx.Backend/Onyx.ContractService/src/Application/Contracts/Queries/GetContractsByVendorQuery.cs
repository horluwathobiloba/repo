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
    public class GetContractsByVendorQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int VendorId { get; set; }
    }


    public class GetContractsByVendorQueryHandler : IRequestHandler<GetContractsByVendorQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractsByVendorQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractsByVendorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //get all supporting documents
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                var entity = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId 
                && a.VendorId == request.VendorId && (!a.IsAnExecutedDocument))
                    .Include(a => a.Vendor)
                    .Include(b => b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Contract), request.VendorId);
                }
                var result = _mapper.Map<List<ContractDto>>(entity);

                foreach (var contract in result)
                {
                    if (supportingDocuments != null && supportingDocuments.Count > 0)
                    {
                        var documents = supportingDocuments.Where(a => a.ContractId == contract.Id);
                        contract.SupportingDocuments = _mapper.Map<List<SupportingDocumentDto>>(documents);
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
