using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Domain.Common;
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
using RubyReloaded.SubscriptionService.Domain.Entities;

namespace RubyReloaded.SubscriptionService.Infrastructure.Persistence
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
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<PaymentChannel> PaymentChannels { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlanPricing> SubscriptionPlanPricings { get; set; }
        public DbSet<SubscriptionPlanFeature> SubscriptionPlanFeatures { get; set; }
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

            builder.Entity<Subscription>()
                .HasIndex(b => b.SubscriptionPlanId);

            //builder.Entity<WorkflowLevel>()
            //    .HasIndex(b => b.WorkflowPhaseId);

            //builder.Entity<WorkflowPhase>()
            //   .HasIndex(b => b.Name);

            //builder.Entity<Contract>()
            //     .Property(p => p.ContractValue)
            //     .HasColumnType("decimal(18,2)")
            //     .HasPrecision(18, 2);

            //builder.Entity<Vendor>()
            //    .HasMany(e => e.Contracts)
            //.WithOne(e => e.Vendor)
            //.OnDelete(DeleteBehavior.SetNull);
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
