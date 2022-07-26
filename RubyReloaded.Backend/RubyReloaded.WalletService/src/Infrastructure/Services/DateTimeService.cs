using RubyReloaded.WalletService.Application.Common.Interfaces;
using System;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
