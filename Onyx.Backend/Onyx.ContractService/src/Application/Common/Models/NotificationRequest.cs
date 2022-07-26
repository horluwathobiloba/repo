using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class NotificationRequest
    {
        public string CustomerId { get; set; }
        public string Message { get; set; }
        //public string DeviceId { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
       
    }
}
