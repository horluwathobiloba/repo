using OnyxDoc.DocumentService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
