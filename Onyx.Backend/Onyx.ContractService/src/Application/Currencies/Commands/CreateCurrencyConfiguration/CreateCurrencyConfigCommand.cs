using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Commands.CreateContract;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit
{
    public class CreateCurrencyConfigCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; }
    }
    public class CreateCurrencyConfigCommandHandler : IRequestHandler<CreateCurrencyConfigCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCurrencyConfigCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateCurrencyConfigCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var newCurrency = new CurrencyConfiguration
                {
                    CreatedBy = request.UserId,
                    OrganisationId = request.OrganisationId,
                    Name = request.Currency,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.CurrencyConfigurations.AddAsync(newCurrency);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Currency Config was created successfully!", newCurrency);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency Config creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
