using Onyx.AuthService.Application.Common.Interfaces;
using System;

namespace Onyx.AuthService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
