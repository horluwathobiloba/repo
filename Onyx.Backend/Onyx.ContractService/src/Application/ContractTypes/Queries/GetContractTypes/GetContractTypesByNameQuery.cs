using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes
{
    public class GetContractTypesByNameQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string Name { get; set; }
    }
    public class GetContractTypesByNameQueryHandler : IRequestHandler<GetContractTypesByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractTypesByNameQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypesByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
            var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.Name))
                {
                    return Result.Failure($"Contract type name must be specified.");
                }
                var list = await _context.ContractTypes.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.Name.IsIn(a.Name)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(ContractType), request.OrganisationId);
                }
                var result = _mapper.Map<List<ContractTypeDto>>(list);
                foreach (var item in result)
                {
                    item.ContractTypeInitiators = await _context.ContractTypeInitiators.Where(a => a.OrganisationId == request.OrganisationId && a.ContractTypeId == item.Id)
                        .ToListAsync();

                }
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving contract type by name. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
