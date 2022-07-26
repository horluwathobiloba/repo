using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OnyxDoc.FormBuilderService.Domain.Constants
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
