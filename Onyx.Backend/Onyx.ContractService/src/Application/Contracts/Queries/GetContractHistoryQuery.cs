﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetContracts
{
    public class GetContractHistoryQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; } 
    }

    public class GetContractHistoryQueryHandler : IRequestHandler<GetContractHistoryQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public GetContractHistoryQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(GetContractHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);  
                var entity = await _context.Contracts.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);
                
                if(entity == null)
                {
                    return Result.Failure("Invalid contract specified!");
                }

                if (entity.InitialContractId > 0)
                {
                    var list = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.InitialContractId == entity.InitialContractId)
                    .Include(a => a.Vendor)
                    .Include(b => b.ProductServiceType)
                    .Include(b => b.PaymentPlan)
                    .Include(b => b.ContractDuration)
                    .ToListAsync(); 

                    if (list == null)
                    {
                        return Result.Success($"No record found", default(List<ContractDto>));
                    }
                    var result = _mapper.Map<List<ContractDto>>(list);
                    return Result.Success($"{list.Count} record(s) found", result);
                }
                else
                {
                    return Result.Success($"No record found", default(List<ContractDto>));
                }               
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract termnination failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
