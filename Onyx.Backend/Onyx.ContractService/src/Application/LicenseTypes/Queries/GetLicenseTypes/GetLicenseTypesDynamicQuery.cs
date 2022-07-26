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

namespace Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes
{
    public class GetLicenseTypesDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }

    public class GetLicenseTypesDynamicQueryHandler : IRequestHandler<GetLicenseTypesDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public GetLicenseTypesDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetLicenseTypesDynamicQuery request, CancellationToken cancellationToken)
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

                var list = await _context.LicenseTypes.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Name)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(LicenseType));
                }
                var result = _mapper.Map<List<LicenseTypeDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving vendor types. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
