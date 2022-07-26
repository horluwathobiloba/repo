﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes
{
    public class GetContractTypeSummary : IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }


    public class GetContractSummaryByIdQueryHandler : IRequestHandler<GetContractTypeSummary, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetContractSummaryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractTypeSummary request, CancellationToken cancellationToken)
        {
            // To do: refactor this code
            var approvedContract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var contract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).ToListAsync();
            var totalContract = contract.Count;
            var inProgressContract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var expiredContract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var pendingContract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var disApprovedContract = await _context.ContractTypes.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();

            return Result.Success(
                new
                {
                    totalContract = totalContract,
                    inProgressContract = inProgressContract,
                    expiredContract = expiredContract,
                    pendingContract = pendingContract,
                    approvedContract = approvedContract,
                    disApprovedContract = disApprovedContract
                });

        }
    }
}
