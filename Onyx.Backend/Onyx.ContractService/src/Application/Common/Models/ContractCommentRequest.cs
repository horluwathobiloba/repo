using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class ContractCommentRequest
    {
        public string Comment { get; set; }
        public ContractCommentType ContractCommentType { get; set; }
    }
}
