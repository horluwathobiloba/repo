using RubyReloaded.AuthService.Application.Common.Interfaces;
using System;

namespace RubyReloaded.AuthService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
