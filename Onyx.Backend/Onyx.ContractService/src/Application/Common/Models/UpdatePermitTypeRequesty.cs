using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdatePermitTypeRequest
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
    }
}