using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using RubyReloaded.WalletService.Domain.Entities;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
       // DbSet<BankConfiguration> BankConfigurations { get; set; }
        DbSet<Domain.Entities.Bank> Banks { get; set; }
        DbSet<Domain.Entities.Account> Accounts { get; set; }
        DbSet<Domain.Entities.VirtualAccount> VirtualAccounts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<BankBeneficiary> BankBeneficiaries { get; set; }
        DbSet<WalletBeneficiary> WalletBeneficiaries { get; set; }
        DbSet<Wishlist> Wishlists { get; set; }
        DbSet<Wallet> Wallets { get; set; }
        DbSet<WalletTransaction> WalletTransactions { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Domain.Entities.PaymentChannel> PaymentChannels { get; set; }
        DbSet<Domain.Entities.Currency> Currencies { get; set; }
        DbSet<CardAuthorization> CardAuthorizations { get; set; }
        DbSet<WithdrawalSetting> WithdrawalSettings { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<ProductInterest> ProductInterests { get; set; }
        DbSet<BankService> BankServices { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<PaymentGatewayService> PaymentGatewayServices { get; set; }
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
