using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RubyReloaded.WalletService.Domain.Entities;

namespace RubyReloaded.WalletService.Infrastructure.Persistence
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
        //public DbSet<Bank> Banks { get; set; }
        public DbSet<Bank> Banks { get; set; } 
        public DbSet<Account> Accounts { get; set; }
       // public DbSet<VirtualAccount> VirtualAccounts { get; set; }
        //public DbSet<ProductCategory> ProductCategoryConfigurations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInterest> ProductInterest { get; set; }
        public DbSet<ProductInterestPeriod> ProductInterestPeriod { get; set; }
       // public DbSet<TransactionService> TransactionService { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<VirtualAccount> VirtualAccounts { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<CardAuthorization> CardAuthorizations { get; set; }
        public  DbSet<Payment> Payments  { get; set; }
        public DbSet<PaymentChannel> PaymentChannels { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<BankBeneficiary> BankBeneficiaries { get; set; }
        public DbSet<WalletBeneficiary> WalletBeneficiaries { get; set; }
        public DbSet<WithdrawalSetting> WithdrawalSettings { get; set; }
        public DbSet<Notification> Notifications { get ; set ; }
        public DbSet<BankService> BankServices { get; set; }
       // public DbSet<TransactionService> TransactionServices { get; set; }
        public DbSet<ProductFundingSource> ProductFundingSources { get; set; }
        public DbSet<ProductInterest> ProductInterests { get; set; }
        public DbSet<ProductSettlementAccount> ProductSettlementAccounts { get; set; }
        public DbSet<PaymentGatewayService> PaymentGatewayServices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
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

