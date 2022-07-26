using Onyx.ContractService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace Onyx.ContractService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Contract> Contracts { get; set; }
        DbSet<PaymentPlan> PaymentPlans { get; set; }
        DbSet<Domain.Entities.ReminderConfiguration> ReminderConfigurations { get; set; }
        DbSet<ProductServiceType> ProductServiceTypes { get; set; }
        DbSet<Dimension> Dimensions { get; set; }
        DbSet<Vendor> Vendors { get; set; }
        DbSet<VendorType> VendorTypes { get; set; }
        DbSet<ContractDocument> ContractDocuments { get; set; }
        DbSet<SupportingDocument> SupportingDocuments { get; set; }
        DbSet<ContractComment> ContractComments { get; set; }
        DbSet<ContractType> ContractTypes { get; set; }
        DbSet<CurrencyConfiguration> CurrencyConfigurations { get; set; }
        DbSet<WorkflowPhase> WorkflowPhases { get; set; }
        DbSet<WorkflowLevel> WorkflowLevels { get; set; }
        DbSet<ContractTypeInitiator> ContractTypeInitiators { get; set; }      
        DbSet<ContractRecipient> ContractRecipients { get; set; }
        DbSet<ContractRecipientAction> ContractRecipientActions { get; set; }
        DbSet<Domain.Entities.ContractDuration> ContractDurations { get; set; }
        DbSet<ReminderRecipient> ReminderRecipients { get; set; }       
        DbSet<LicenseType> LicenseTypes { get; set; }
        DbSet<PermitType> PermitTypes { get; set; }
        DbSet<Domain.Entities.ContractTag> ContractTags { get; set; }
        DbSet<Domain.Entities.ContractTask> ContractTasks { get; set; }
        DbSet<Domain.Entities.AuditLog> AuditLogs { get; set; }
        DbSet<Domain.Entities.ReportValue> ReportValues { get; set; }
        DbSet<ContractTaskAssignee> ContractTaskAssignees { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Inbox> Inboxes { get; set; }
        DbSet<WeeklyReminderSchedule> WeeklyReminderSchedules { get; set; }
        DbSet<YearlyReminderSchedule> YearlyReminderSchedules { get; set; }


        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync();
        void RollbackTransaction();
        void Entry<TEntity>(TEntity entity, System.Action<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity>> action) where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int ExecuteSqlRaw(string sql, params object[] parameters);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
        int ExecuteSqlInterpolated(FormattableString sql);
        Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default);

    }
}
