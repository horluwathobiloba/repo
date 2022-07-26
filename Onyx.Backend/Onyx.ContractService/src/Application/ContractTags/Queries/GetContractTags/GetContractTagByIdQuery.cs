using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTag.Queries.GetContractTags
{
    public class GetContractTagByIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }

    public class GetContractTagByIdQueryHandler : IRequestHandler<GetContractTagByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractTagByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractTagByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                } 

                var ContractTagType = await _context.ContractTags.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);
                if (ContractTagType == null)
                {
                    throw new NotFoundException(nameof(ContractTag));
                }

                var result = _mapper.Map<ContractTagDto>(ContractTagType);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving Contract Tag . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
