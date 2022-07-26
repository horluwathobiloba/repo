using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
    public class ContractRecipientsVm
    {
        public string Email { get; set; }

        public string SigningAppUrl { get; set; }

        public string EmailResponse { get; set; }
    }
}
