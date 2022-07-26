using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetContracts
{
    public class GetExecutedPermitsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetExecutedPermitsDynamicQueryHandler : IRequestHandler<GetExecutedPermitsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetExecutedPermitsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetExecutedPermitsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = (await _context.Contracts
                    .Where(a => a.OrganisationId == request.OrganisationId && a.IsAnExecutedDocument && a.DocumentType == DocumentType.Permit)
                    .ToListAsync())
                    .Where(a => request.SearchText.IsIn(a.RoleName)
                    || request.SearchText.IsIn(a.Name)
                    || request.SearchText.IsIn(a.ContractStatus.ToString())
                    || request.SearchText.IsIn(a.OrganisationName)
                    || request.SearchText.IsIn(a.Email)
                    || request.SearchText.IsIn(a.SupplierClass)
                    || request.SearchText.IsIn(a.SupplierCode)
                    || request.SearchText.IsIn(a.ShortName)
                    || request.SearchText.IsIn(a.State)
                    || request.SearchText.IsIn(a.Country));
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

                return Result.Success($"{list.Count()} record(s) found", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving workflow phases. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
