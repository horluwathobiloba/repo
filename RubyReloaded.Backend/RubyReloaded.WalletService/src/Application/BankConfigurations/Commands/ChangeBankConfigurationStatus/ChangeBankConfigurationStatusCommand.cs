using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models; 
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Commands.ChangeBankConfigurationStatus
{
    public class ChangeBankConfigurationStatusCommand: IRequest<Result>
    {
        public int BankConfigurationId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeBankConfigurationStatusCommandHandler : IRequestHandler<ChangeBankConfigurationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeBankConfigurationStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeBankConfigurationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.BankConfigurations.FindAsync(request.BankConfigurationId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Bank Configurationuration deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Bank Configurationuration activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Bank Configurationuration activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Bank Configurationuration status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
