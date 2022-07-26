using Onyx.WorkFlowService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Enums;

namespace Onyx.WorkFlowService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
      
        DbSet<Organization> Organizations { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<StaffCount> StaffCount { get; set; }

        DbSet<RolePermission> RolePermissions { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
