using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class CreateInboxRequest
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Body { get; set; }
        public string ReciepientEmail { get; set; }
        public EmailAction EmailAction { get; set; }
        public bool Delivered { get; set; }
        public string Email { get; set; }
    }
}
