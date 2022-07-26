using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Common.Interfaces
{
   public interface INotificationService
   {
        Task SendNotification(string deviceID, string message);
   }
}
