using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enitities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Commands.CreateBankConfiguration
{
    public class BankDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class CreateBankConfigurationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public List<BankDto> BankDtos { get; set; }
    }

    public class CreateBankConfigurationCommandHandler : IRequestHandler<CreateBankConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
        private readonly IConfiguration _configuration;
        public CreateBankConfigurationCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient,
        IConfiguration configuration)
        {
            _context = context;
            _aPIClient = aPIClient;
            _configuration = configuration;
    }
        public async Task<Result> Handle(CreateBankConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //string apiUrl = _configuration["UserValidation:Url"];
                //var requestBody = new { UserId = request.UserId };
                //var response = await _aPIClient.Post(apiUrl, "", requestBody);                

                await _context.BeginTransactionAsync();
                var bankConfigurations = new List<BankConfiguration>();
                //get all banks
                var banks = await _context.BankConfigurations.Where(x => x.Status == Status.Active).ToDictionaryAsync(a=>a.Name);
               
                foreach (var item in request.BankDtos)
                {
                    if (banks != null || banks.Count() > 0)
                    {
                        if (banks.TryGetValue(item.Name, out BankConfiguration bank))
                        {
                            if (bank != null)
                            {
                                if (request.BankDtos.Count() == 1)
                                {
                                    return Result.Failure(new string[] { "Bank Configurations already exist" });
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        
                    }
                   

                    var bankConfiguration = new BankConfiguration
                    {
                        Name = item.Name,
                        ShortName = item.ShortName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                    };
                    bankConfigurations.Add(bankConfiguration);
                }
                
                await _context.BankConfigurations.AddRangeAsync(bankConfigurations);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(bankConfigurations);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Bank Configurationuration creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
