﻿
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class PaystackPlanVm
    {
        //
        // Summary:
        //     (ID of the Customer) The ID of the customer for this Session. For Checkout Sessions
        //     in payment or subscription mode, Checkout will create a new customer object based
        //     on information provided during the payment flow unless an existing customer was
        //     provided when the Session was created. 
         
        public int SubscriberId { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }

        [NotMapped]
        public PaystackAccountVm Plan { get; set; }       
    }
}
