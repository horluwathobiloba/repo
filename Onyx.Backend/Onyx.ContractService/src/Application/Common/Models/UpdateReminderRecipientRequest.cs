using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateReminderRecipientRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }

    }
}
