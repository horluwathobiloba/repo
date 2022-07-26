using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Domain.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface ITransactionService
    {
        Task<string> CreateTransaction(TransactionRequest request, CancellationToken cancellationToken);
    }
}
