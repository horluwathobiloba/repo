using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ReminderConfiguration : AuditableEntity
    {
        public DateTime  StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecurrenceValue { get; set; }
        public ReminderScheduleFrequency ReminderScheduleFrequency { get; set; }
        public List<WeeklyReminderSchedule> WeeklyReminderSchedule { get; set; }
        public string MonthlyReminderScheduleValue { get; set; }
        public YearlyReminderSchedule YearlyReminderSchedule { get; set; }
        public ReminderType ReminderType { get; set; }
        public int RecurringCountBalance { get; set; }
        public DateTime NextRecurringPeriod { get; set; }
    }
}
