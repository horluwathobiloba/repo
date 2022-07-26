using Onyx.ContractService.Application.Common.Interfaces;
using System;

namespace Onyx.ContractService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
