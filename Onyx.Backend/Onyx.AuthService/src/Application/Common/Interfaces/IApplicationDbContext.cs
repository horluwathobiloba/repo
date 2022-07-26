using Onyx.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
      
        DbSet<Organization> Organizations { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<UserCount> UserCount { get; set; }

        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        DbSet<Domain.Entities.UserRefreshToken> UserRefreshTokens { get; set; }
        DbSet<Domain.Entities.JobFunction> JobFunctions { get; set; }
       
        
        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void RollbackTransaction();
    }
}
