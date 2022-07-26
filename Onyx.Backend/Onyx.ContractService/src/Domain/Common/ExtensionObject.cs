using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Common
{
    public static class ExtensionObject
    {
        public static DateTime ComputeContractExpirationDate(this Domain.Entities.Contract contract)
        {
            try
            {
                var duration = contract.ContractDuration;
                switch (duration?.DurationFrequency)
                {
                    case DurationFrequency.Day:
                        {
                            return contract.ContractStartDate.Value.AddDays(duration.Duration);
                        }
                    case DurationFrequency.Month:
                        {
                            return contract.ContractStartDate.Value.AddMonths(duration.Duration);
                        }
                    case DurationFrequency.Week:
                        {
                            return contract.ContractStartDate.Value.AddDays(duration.Duration * 7);
                        }
                    case DurationFrequency.Year:
                        {
                            return contract.ContractStartDate.Value.AddYears(duration.Duration);
                        }
                    default:
                        return contract.ContractStartDate.Value.AddYears(1);
                }
            }
            catch (Exception ex)
            {
                return contract.ContractStartDate.Value.AddYears(1);  // default
            }
        }

      
    }
}
