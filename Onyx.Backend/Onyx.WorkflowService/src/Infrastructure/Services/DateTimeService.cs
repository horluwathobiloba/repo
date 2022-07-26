using Onyx.WorkFlowService.Application.Common.Interfaces;
using System;

namespace Onyx.WorkFlowService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
