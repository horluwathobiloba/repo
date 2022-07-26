using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using RubyReloaded.SubscriptionService.Domain.Entities;

namespace RubyReloaded.SubscriptionService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Currency> Currencies { get; set; }
        DbSet<PaymentChannel> PaymentChannels { get; set; }
        DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<SubscriptionPlanPricing> SubscriptionPlanPricings { get; set; }
        DbSet<SubscriptionPlanFeature> SubscriptionPlanFeatures { get; set; }

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
