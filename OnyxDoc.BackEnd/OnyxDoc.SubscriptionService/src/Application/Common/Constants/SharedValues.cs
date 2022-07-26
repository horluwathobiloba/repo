using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Constants
{
    public class SharedValues
    {
        
        public static string PayStack
        {
            get
            {
                return null;//  new Configuration().GetSection["Paystack:Key"];
            }
        } 
    }
}
