using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Commands;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RubyReloaded.WalletService.Application.Cards.Commands
{
    public class AddCardAuthorizationCommand : AuthToken, IRequest<Result>
    {
        public string Email { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string UserId { get; set; }
    }

    public class AddCardAuthorizationCommandHandler : IRequestHandler<AddCardAuthorizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IPaystackService _paystackService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;

        public AddCardAuthorizationCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                         IPaystackService paystackService, IStringHashingService stringHashingService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _paystackService = paystackService;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(AddCardAuthorizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new InitiliazePaystackPaymentCommand
                {
                    Amount = Convert.ToInt32(_configuration["Paystack:CardAuthorizationCharge"]),//kobo equivalent,
                    CurrencyCode = request.CurrencyCode,
                    Email = request.Email,
                    SaveCardDetails = true,
                    WalletId = 0,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                var handler = new InitiliazePaystackPaymentCommandHandler(_context, _mapper, _authService,_paystackService,_stringHashingService,_configuration);
                var initializeFirstCardTransactionResult = await handler.Handle(command, cancellationToken);
                if (initializeFirstCardTransactionResult.Succeeded == false)
                {
                    throw new Exception(initializeFirstCardTransactionResult.Error + initializeFirstCardTransactionResult.Message);
                }
                return Result.Success(initializeFirstCardTransactionResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Add card on Authorization failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
