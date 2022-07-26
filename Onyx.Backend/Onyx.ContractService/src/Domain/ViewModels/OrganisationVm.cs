using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
        public class OrganisationVm
        {
            public bool Succeeded { get; set; }
            public OrganisationDto Entity { get; set; }
            public string[] Messages { get; set; }
        }    
}
