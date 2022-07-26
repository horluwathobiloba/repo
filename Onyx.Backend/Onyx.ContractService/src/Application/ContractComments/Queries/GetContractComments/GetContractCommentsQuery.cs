using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractComments.Queries.GetContractComments
{
    public class GetContractCommentsQuery:IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetContractCommentsQueryHandler : IRequestHandler<GetContractCommentsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractCommentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                } 

                var list = await _context.ContractComments.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                var result = _mapper.Map<List<ContractCommentDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving contract comments. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
