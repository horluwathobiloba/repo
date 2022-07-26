using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.ChangeProductCategoryConfigurationStatus
{
    public class ChangeProductCategoryConfigurationStatusCommand: IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeProductCategoryConfigurationStatusCommandHandler : IRequestHandler<ChangeProductCategoryConfigurationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeProductCategoryConfigurationStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeProductCategoryConfigurationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.ProductCategoryConfigurations.FindAsync(request.Id);
                if (entity == null)
                {
                    return Result.Failure(new string[] {"Product Category Configuration Id not found" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Product Category Configuration deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Product Category Configuration activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Product Category Configuration activation was successful";
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
                return Result.Failure(new string[] { "Product Category Configuration status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
