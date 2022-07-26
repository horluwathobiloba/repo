using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace Onyx.ContractService.Application.Vendors.Queries.GetVendors
{
    public class GetVendorsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }

    public class GetVendorsDynamicQueryHandler : IRequestHandler<GetVendorsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetVendorsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetVendorsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.SearchText))
                {
                    return Result.Failure($"Search text must be specified.");
                }

                var list = await _context.Vendors.Include(a => a.VendorType).Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Name)
                || request.SearchText.IsIn(a.State)
                || request.SearchText.IsIn(a.ShortName)
                || request.SearchText.IsIn(a.SupplierCode)
                || request.SearchText.IsIn(a.PhoneNumber)
                || request.SearchText.IsIn(a.Email)
                || request.SearchText.IsIn(a.Country)
                || request.SearchText.IsIn(a.SupplierClass)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(Vendor));
                }
                var result = _mapper.Map<List<VendorDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving vendors. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
