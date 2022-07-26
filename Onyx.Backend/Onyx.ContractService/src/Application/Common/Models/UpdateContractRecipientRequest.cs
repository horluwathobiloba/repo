using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateContractRecipientRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }
        public RecipientCategory RecipientCategory { get; set; }
        
    }
}
