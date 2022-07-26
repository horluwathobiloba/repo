using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class GetContractRecipientQuery : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }


    public class GetContractRecipientQueryHandler : IRequestHandler<GetContractRecipientQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractRecipientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractRecipientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractRecipients.Where(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id)
                    .Include(a => a.Contract) 
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractRecipient), request.Id);
                }
                var result = _mapper.Map<ContractRecipientDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract recipient by id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
