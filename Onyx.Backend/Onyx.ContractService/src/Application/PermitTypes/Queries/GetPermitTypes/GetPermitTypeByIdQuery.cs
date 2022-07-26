using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes
{
    public class GetPermitTypeByIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }

    public class GetPermitTypeByIdQueryHandler : IRequestHandler<GetPermitTypeByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPermitTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPermitTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                } 
                var vendorType = await _context.PermitTypes.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);

                if (vendorType == null)
                {
                    throw new NotFoundException(nameof(PermitType));
                }
                var result = _mapper.Map<PermitTypeDto>(vendorType);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving license type. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
