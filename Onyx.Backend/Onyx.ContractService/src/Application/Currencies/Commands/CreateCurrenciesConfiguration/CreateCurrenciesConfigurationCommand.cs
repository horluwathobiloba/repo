using AutoMapper;
using MediatR;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Currencies.Commands.CreateCurrenciesConfiguration
{
    public class CreateCurrenciesConfigurationCommand : IRequest<Result>
    {
        public List<string> Currencies { get; set; }
        public string UserId { get; set; }
        public int OrganisationId { get; set; }
    }

    public class CreateCurrenciesConfigurationCommandHandler : IRequestHandler<CreateCurrenciesConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CreateCurrenciesConfigurationCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateCurrenciesConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<Domain.Entities.CurrencyConfiguration>();
                await _context.BeginTransactionAsync();
                foreach (var currency in request.Currencies)
                {
                    var entity = new Domain.Entities.CurrencyConfiguration
                    {
                        CreatedBy = request.UserId,
                        OrganisationId = request.OrganisationId,
                        Name = currency,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.CurrencyConfigurations.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Currencies created successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency Config creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}

