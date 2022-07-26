using Newtonsoft.Json;
using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class RoleDto : AuthEntity
    {
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }
        public OrganisationDto Organization { get; set; }
    }
}
