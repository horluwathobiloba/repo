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
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;

namespace Onyx.ContractService.Application.ContractTags.Queries.GetContractTags
{
    public class GetContractTagsDynamicQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }

    public class GetContractTagsDynamicQueryHandler : IRequestHandler<GetContractTagsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractTagsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractTagsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.SearchText))
                {
                    return Result.Failure($"Search text must be specified.");
                }

                var list = await _context.ContractTags.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Name)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(ContractTag));
                }
                var result = _mapper.Map<List<ContractTagDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving ContractTag types. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
