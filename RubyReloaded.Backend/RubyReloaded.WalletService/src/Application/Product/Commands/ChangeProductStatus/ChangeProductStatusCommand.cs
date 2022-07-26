using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Commands.ChangeProductStatus
{
    public class ChangeProductStatusCommand: IRequest<Result>
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeProductStatusCommandHandler : IRequestHandler<ChangeProductStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeProductStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeProductStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.Products.FindAsync(request.ProductId);
                if (entity == null)
                {
                    return Result.Failure(new string[] {"Product Id not found" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Product deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Product activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Product activation was successful";
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
                return Result.Failure(new string[] { "Product  status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
