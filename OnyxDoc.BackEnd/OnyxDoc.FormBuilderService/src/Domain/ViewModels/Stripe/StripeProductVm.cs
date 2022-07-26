using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class StripeProductVm
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public StripeAccountVm Plan { get; set; }
    }
}
