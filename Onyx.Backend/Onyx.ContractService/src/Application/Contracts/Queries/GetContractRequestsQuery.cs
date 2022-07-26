﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries
{
    public class GetContractRequestsQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetContractRequestsQueryHandler : IRequestHandler<GetContractRequestsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractRequestsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractRequestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                //get all supporting documents
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                var list = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.DocumentType==Domain.Enums.DocumentType.Contract &&(!a.IsAnExecutedDocument))
                    .Include(a => a.Vendor)
                    .Include(b => b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .ToListAsync();


                //this is because when we add include for objects that are null, it doesnt return any value
                var saveAsDraftContractRequests = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && (!a.IsAnExecutedDocument)
                  && (a.VendorId == 0 || a.ProductServiceTypeId == 0 || a.PaymentPlanId == 0 || a.ContractDurationId == null) && a.ContractStatus == Domain.Enums.ContractStatus.Processing)
                 .ToListAsync();
                list.AddRange(saveAsDraftContractRequests);
                list.OrderBy(a => a.CreatedDate);
                if (list == null)
                {
                    throw new NotFoundException(nameof(Contract));
                }

                var result = _mapper.Map<List<ContractDto>>(list);

                //this code needs to be refactored @Oluchi
                foreach (var contract in result)
                {
                    if (supportingDocuments != null && supportingDocuments.Count > 0)
                    {
                        var documents = supportingDocuments.Where(a => a.ContractId == contract.Id);
                        contract.SupportingDocuments = _mapper.Map<List<SupportingDocumentDto>>(documents);
                    }
                }

                return Result.Success($"{list.Count} record(s) found", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
