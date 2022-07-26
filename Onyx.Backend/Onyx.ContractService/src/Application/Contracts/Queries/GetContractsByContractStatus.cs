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
    public class GetContractsByContractStatusQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public ContractStatus ContractStatus { get; set; }
    }


    public class GetContractsByContractStatusQueryHandler : IRequestHandler<GetContractsByContractStatusQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractsByContractStatusQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractsByContractStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get all supporting documents
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                var entity = await _context.Contracts
                    .Where(a => a.OrganisationId == request.OrganisationId && a.ContractStatus == request.ContractStatus && (!a.IsAnExecutedDocument))
                      .Include(a => a.Vendor)
                      .Include(b => b.ProductServiceType)
                      .Include(b => b.PaymentPlan)
                      .Include(b => b.ContractDuration)
                      .ToListAsync();

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Contract), request.ContractStatus);
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
                return Result.Failure($"Error retrieving contract by ContractStatus. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
