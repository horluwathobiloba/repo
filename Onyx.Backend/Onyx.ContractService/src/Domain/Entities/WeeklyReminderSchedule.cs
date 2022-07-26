using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class WeeklyReminderSchedule : AuditableEntity
    {
        public string Day { get; set; }
        public int ReminderConfigurationId { get; set; }
    }
}
