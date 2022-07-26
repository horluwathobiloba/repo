using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateContractDurationRequest
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
    }
}
