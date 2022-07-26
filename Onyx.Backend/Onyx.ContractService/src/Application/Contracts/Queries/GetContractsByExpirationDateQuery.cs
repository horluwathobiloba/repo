using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries
{
    public class GetContractsByExpirationDateQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetContractsByExpirationDateQueryHandler : IRequestHandler<GetContractsByExpirationDateQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetContractsByExpirationDateQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetContractsByExpirationDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //var contracts = await _context.Contracts.Where(a=>a.OrganisationId == request.OrganisationId && 
                //                                         (a.ContractStatus == Domain.Enums.ContractStatus.Active 
                //                                         || a.ContractStatus == Domain.Enums.ContractStatus.Expired)
                //                                         && a.ContractExpirationDate.HasValue).
                //OrderByDescending(x => x.ContractExpirationDate).Select(x => new ContractsVm
                //{
                //    VendorName = x.Vendor.Name,
                //    DocumentType = x.DocumentType,
                //    CreatedDate = x.CreatedDate,
                //    DocumentTypeDesc = x.DocumentTypeDesc,
                //    ExpirationDate = (DateTime)x.ContractExpirationDate,
                //    TimeLeft = x.ContractExpirationDate.HasValue ? x.ContractExpirationDate.Value.Day - DateTime.Now.Day : 0
                //}).ToListAsync();


                var contracts = await _context.Contracts.Where(a=>a.OrganisationId == request.OrganisationId && 
                                                         (a.ContractStatus == Domain.Enums.ContractStatus.Active 
                                                         || a.ContractStatus == Domain.Enums.ContractStatus.Expired)
                                                         && a.ContractExpirationDate.HasValue).
                OrderByDescending(x => x.ContractExpirationDate).Select(x => new ContractsVm
                {
                    VendorName = x.Vendor.Name,
                    DocumentType = x.DocumentType,
                    CreatedDate = x.CreatedDate,
                    DocumentTypeDesc = x.DocumentTypeDesc,
                    ExpirationDate = (DateTime)x.ContractExpirationDate,
                    //TimeLeft = x.ContractExpirationDate.HasValue ? Convert.ToInt32((x.ContractExpirationDate.Value - DateTime.Now).TotalDays) : 0
                    TimeLeft = x.ContractExpirationDate.HasValue ? Convert.ToInt32((x.ContractExpirationDate.Value.Subtract(DateTime.Now)).TotalDays) : 0
                }).ToListAsync();




                if (!contracts.Any())
                {
                    return Result.Failure("There is no existing contract in the database");
                }

                return Result.Success($"Retrieving contract by expiration date was successful, {contracts.Count} record(s) found. ",contracts);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving contract by expiration date. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
