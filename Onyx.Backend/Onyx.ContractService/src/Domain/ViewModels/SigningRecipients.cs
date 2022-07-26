using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
    public class SigningRecipients
    {
        public string RecipientEmail { get; set; }
        public DimensionVm[] Dimensions { get; set; }
        //public int Rank { get; set; }

    }
}