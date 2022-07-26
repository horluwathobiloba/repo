using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Common.Interfaces
{
    public interface IReminderScheduleService
    {
        Task<Domain.Entities.ReminderConfiguration> ComputeReminderSchedule(Domain.Entities.ReminderConfiguration reminderConfiguration);
    }
}
