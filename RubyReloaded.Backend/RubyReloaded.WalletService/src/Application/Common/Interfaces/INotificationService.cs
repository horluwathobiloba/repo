using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface INotificationService
   {
        Task SendNotification(string deviceID, string message);
   }
}
