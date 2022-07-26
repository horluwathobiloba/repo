using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class GetContractRecipientsDynamicQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetContractRecipientsDynamicQueryHandler : IRequestHandler<GetContractRecipientsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractRecipientsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractRecipientsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = (await _context.ContractRecipients
                    .Where(a => a.OrganisationId == request.OrganisationId)
                    .Include(a => a.Contract)
                    .ToListAsync())
                    .Where(a => request.SearchText.IsIn(a.Email) || request.SearchText.IsIn(a.Contract.Name));

                if (list == null)
                {
                    throw new NotFoundException(nameof(ContractRecipient));
                }
                var result = _mapper.Map<List<ContractRecipientDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving workflow phases. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
