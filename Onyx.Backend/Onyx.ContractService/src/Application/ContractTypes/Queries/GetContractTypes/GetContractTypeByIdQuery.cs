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

namespace Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes
{
    public class GetContractTypeByIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }


    public class GetProductsByIdQueryHandler : IRequestHandler<GetContractTypeByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetProductsByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                var entity = await _context.ContractTypes.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractType), request.Id);
                }
                var result = _mapper.Map<ContractTypeDto>(entity);
                result.ContractTypeInitiators = await _context.ContractTypeInitiators.Where(a => a.OrganisationId == request.OrganisationId && a.ContractTypeId == entity.Id)
                        .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving contract type by id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
