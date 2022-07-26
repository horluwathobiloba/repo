using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace Onyx.ContractService.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        #region Model

        public DbSet<ReminderConfiguration> ReminderConfigurations { get; set; }
        public DbSet<Domain.Entities.Contract> Contracts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ContractRecipient> ContractRecipients { get; set; }
        public DbSet<ContractRecipientAction> ContractRecipientActions { get; set; }
        public  DbSet<CurrencyConfiguration> CurrencyConfigurations{ get; set; }
        public DbSet<Dimension> Dimensions { get; set; }
        public DbSet<WorkflowPhase> WorkflowPhases { get; set; }
        public DbSet<WorkflowLevel> WorkflowLevels { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<ProductServiceType> ProductServiceTypes { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
        public DbSet<ContractDocument> ContractDocuments { get; set; }
        public DbSet<ContractComment> ContractComments { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<PermitType> PermitTypes { get; set; }
        public DbSet<ContractTypeInitiator> ContractTypeInitiators { get; set; } 
        
        public DbSet<LicenseType> LicenseTypes { get; set; }
        public DbSet<ReminderRecipient> ReminderRecipients { get; set; }

        public DbSet<ContractDuration> ContractDurations { get; set; }
        public DbSet<ContractTag> ContractTags { get; set; }

        // public DbSet<Domain.Entities.ContractTag> ContractTags { get; set; }
        public DbSet<ContractTask> ContractTasks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ReportValue> ReportValues { get; set; }
        public DbSet<ContractTaskAssignee> ContractTaskAssignees { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<WeeklyReminderSchedule> WeeklyReminderSchedules { get; set; }
        public DbSet<YearlyReminderSchedule> YearlyReminderSchedules { get; set; }
        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        // entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModifiedDate = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region RawSql

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await base.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return await base.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public int ExecuteSqlInterpolated(FormattableString sql)
        {
            return base.Database.ExecuteSqlInterpolated(sql);
        }

        public async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default)
        {
            return await base.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);
        }

        #endregion

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public virtual void Entry<TEntity>(TEntity entity, Action<EntityEntry<TEntity>> action) where TEntity : class
        {
            action(base.Entry(entity));
        }

        //[Obsolete("Use overload for unit tests.")]
        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
            ///** or **/
            //throw new ApplicationException("Use overload for unit tests.");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            builder.Entity<WorkflowLevel>()
                .HasIndex(b => b.WorkflowPhaseId);

            builder.Entity<WorkflowPhase>()
               .HasIndex(b => b.Name);

            builder.Entity<Domain.Entities.Contract>()
                 .Property(p => p.ContractValue)
                 .HasColumnType("decimal(18,2)")
                 .HasPrecision(18, 2);

            builder.Entity<Domain.Entities.Vendor>()
                .HasMany(e => e.Contracts)
            .WithOne(e => e.Vendor)
            .OnDelete(DeleteBehavior.SetNull);
        }

        //Implementation of 
        public async Task<List<T>> SqlQueryAsync<T>(string query, Func<DbDataReader, T> map)
        {
            try
            {
                return await this.SqlQueryAsync<T>(query, map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> SqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            try
            {
                return this.SqlQuery<T>(query, map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> ExecuteSqlCommandAsync<T>(string query, bool InvokeTxn = false)
        {
            try
            {
                return await this.ExecuteSqlCommandAsync<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ExecuteSqlCommand<T>(string query, bool InvokeTxn = false)
        {
            try
            {
                return this.ExecuteSqlCommand<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
