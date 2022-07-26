using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateWorkflowLevelRequest
    {
        public int Id { get; set; } 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Rank { get; set; }
        public WorkflowLevelAction WorkflowLevelAction { get; set; }

        internal RoleDto Role { get; set; }
    }
}