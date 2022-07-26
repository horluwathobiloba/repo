using OnyxDoc.AuthService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
