using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.ChangeVirtualAccountConfigStatus
{
    public class ChangeVirtualAccountConfigurationStatusCommand : IRequest<Result>
    {
        public int VirtualAccountConfigId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeVirtualAccountConfigurationStatusCommandHandler : IRequestHandler<ChangeVirtualAccountConfigurationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeVirtualAccountConfigurationStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeVirtualAccountConfigurationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.VirtualAccountConfigurations.FindAsync(request.VirtualAccountConfigId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Virtual Account Configuration deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Virtual Account Configuration activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Virtual Account Configuration activation was successful";
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
                return Result.Failure(new string[] { "Virtual Account Configuration status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
