using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Application.Common.Interfaces
{
   public interface ITokenService
    {
            AuthToken GenerateToken(Staff staff, List<RolePermission> rolePermissions);
    }
}
