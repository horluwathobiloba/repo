using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using System;

namespace OnyxDoc.FormBuilderService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
