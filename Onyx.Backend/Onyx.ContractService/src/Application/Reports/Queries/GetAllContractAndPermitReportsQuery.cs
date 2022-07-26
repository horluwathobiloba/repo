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

namespace Onyx.ContractService.Application.Reports.Queries.GetReports
{
    public class GetAllContractAndPermitReportsQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public int ProductType { get; set; }
        public int JobFunction { get; set; }
        public int ContractType { get; set; }
    }

    public class GetAllContractAndPermitReportsQueryHandler : IRequestHandler<GetAllContractAndPermitReportsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetAllContractAndPermitReportsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetAllContractAndPermitReportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = (await _context.Contracts
                    .Where(a => a.OrganisationId == request.OrganisationId)
                    .ToListAsync())
                    .Where(a=>request.Status.ToString().Contains(a.ContractStatus.ToString())
                    || request.StartDate.ToString().Contains(a.ContractStartDate.ToString())
                    || request.EndDate.ToString().Contains(a.ContractExpirationDate.ToString())
                    || request.ProductType.ToString().IsIn(a.ProductServiceTypeId.ToString())
                    || request.ContractType.ToString().IsIn(a.ContractTypeId.ToString()));


                    //(a => request.SearchText.Contains(a.CreatedDate.ToString())
                    //|| request.SearchText.Contains(a.ContractExpirationDate.ToString())
                    //|| request.SearchText.Contains(a.ContractStatus.ToString())
                    //|| request.SearchText.Contains(a.DocumentType.ToString())
                    //|| request.SearchText.IsIn(a.Vendor.Name)
                    //|| request.SearchText.IsIn(a.ProductServiceType.Name)
                    //|| request.SearchText.IsIn(a.OrganisationName));

                if (list == null)
                {
                    throw new NotFoundException(nameof(Contract));
                }
                var result = _mapper.Map<List<ContractDto>>(list);
               
                return Result.Success($"{list.Count()} record(s) found", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving contract reports . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}