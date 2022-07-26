using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IConvertWordToPdfService
    {
        Task<string> ConvertWordToPdf(string fileName);
    }
}
