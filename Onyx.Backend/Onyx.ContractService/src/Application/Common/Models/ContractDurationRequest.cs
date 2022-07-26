using MediatR;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class ContractDurationRequest
    {
        public int Duration { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
    }
}
