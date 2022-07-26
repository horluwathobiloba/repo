using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class Inbox:AuditableEntity
    {
        public string Body { get; set; }
        public string ReciepientEmail { get; set; }
        public bool Delivered { get; set; }
        public bool Read { get; set; }
        public EmailAction EmailAction { get; set; }
        public string Sender { get; set; }

    }
}
