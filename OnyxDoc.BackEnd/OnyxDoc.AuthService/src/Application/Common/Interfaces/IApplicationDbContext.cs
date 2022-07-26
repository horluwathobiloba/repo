using OnyxDoc.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
      
        DbSet<Subscriber> Subscribers { get; set; }
        DbSet<UserInviteLink> UserInviteLinks { get; set; }
        DbSet<UserApproverRequest> UserApproverRequests { get; set; }

        DbSet<SubscriberCount> SubscribersCount { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<UserCount> UserCount { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<Feature> Features { get; set; }
        DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        DbSet<Domain.Entities.Branding> Brandings { get; set; }
        DbSet<Domain.Entities.SystemSetting> SystemSettings { get; set; }
        DbSet<Domain.Entities.ExpiryPeriod> ExpiryPeriods { get; set; }
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<DefaultRolesConfiguration> DefaultRolesConfigurations { get; set; }
        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void RollbackTransaction();
    }
}
