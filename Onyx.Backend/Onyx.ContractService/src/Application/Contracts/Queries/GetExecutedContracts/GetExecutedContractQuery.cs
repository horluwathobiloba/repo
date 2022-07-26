using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetExecutedContracts
{
    public class GetExecutedContractQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }


    public class GetExecutedContractQueryHandler : IRequestHandler<GetExecutedContractQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetExecutedContractQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetExecutedContractQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id && a.IsAnExecutedDocument)
                    .Include(a => a.Vendor)
                    .Include(b => b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Contract), request.Id);
                }
                var result = _mapper.Map<ContractDto>(entity);

                //get all supporting documents in the organisation and contract
                var supportingDocuments = await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id).ToListAsync();
                List<SupportingDocumentDto> documents = new List<SupportingDocumentDto>();
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    documents = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                }
                result.SupportingDocuments = documents;

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract by id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
