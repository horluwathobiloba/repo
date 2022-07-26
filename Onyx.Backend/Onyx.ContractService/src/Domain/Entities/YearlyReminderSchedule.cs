using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class YearlyReminderSchedule : AuditableEntity
    {
        public int Value { get; set; }
        public string Month { get; set; }
        public int ReminderConfigurationId { get; set; }
    }
}
