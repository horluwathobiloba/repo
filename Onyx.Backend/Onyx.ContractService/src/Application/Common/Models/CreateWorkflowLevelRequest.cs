using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;

namespace Onyx.ContractService.Application.Common.Models
{
    public class CreateWorkflowLevelRequest
    {        
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Rank { get; set; }
        public WorkflowLevelAction WorkflowLevelAction { get; set; }

        internal RoleDto Role { get; set; }
    }
}