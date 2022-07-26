using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Infrastructure.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        public DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        //public DbSet<CooperativeUserMapping> CooperativeUserMappings { get; set; }
        public DbSet<UserLinkInvite> UserLinkCreations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PermissionSet> PermissionSets  { get; set; }
        public DbSet<UserCount> UserCount { get; set; }
        public DbSet<Cooperative> Cooperatives { get; set; }
        public DbSet<CooperativeRole> Roles { get; set; }
        public DbSet<CooperativeRolePermission> RolePermissions { get; set; }
        public DbSet<CooperativeSettings> CooperativeSettings { get;set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Ajo> Ajos { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AjoMember> AjoMembers { get; set ; }
        public DbSet<AjoCode> AjoCodes { get; set; }
        public DbSet<CooperativeUserCode> CooperativeUserCodes { get; set; }
        public DbSet<RequestToJoinTracker> RequestToJoinTrackers { get; set; }
        public DbSet<AjoInvitationTracker> AjoInvitationTrackers { get; set ; }
        public DbSet<CooperativeInvitationTracker> CooperativeInvitationTrackers { get; set ; }
        public DbSet<SystemOwner> SystemOwners { get; set; }
        public DbSet<AjoRole> AjoRoles { get; set; }
        public DbSet<SystemOwnerRole> SystemOwnerRoles { get; set; }
        public DbSet<AjoRolePermission> AjoRolePermissions { get; set ; }
        public DbSet<SystemOwnerRolePermission> SystemOwnerRolePermissions { get; set; }
        public DbSet<CooperativeUserMapping> CooperativeMembers { get; set; }
        public DbSet<SystemOwnerUsers> SystemOwnerUsers { get; set ; }
        public DbSet<CurrencyConfiguration> CurrencyConfigurations { get ; set; }
        public DbSet<PaymentChannel> PaymentChannels { get; set; }
        public DbSet<FAQCategory> FAQCategories { get ; set ; }
        public DbSet<FAQQuestion> FAQQuestions { get ; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagFAQ> TagFAQs { get; set; }
        public DbSet<ExploreCategory> ExploreCategories { get ; set ; }
        public DbSet<ExplorePost> ExplorePosts { get ; set; }
        public DbSet<ExplorePostFile> ExplorePostFiles { get ; set; }
        public DbSet<ExploreTag> ExploreTags { get ; set ; }
        public DbSet<ExploreTagPost> ExploreTagPosts { get; set ; }

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
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModifiedDate = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<ExplorePostFile>().Property(p=>p.ExplorePostFileURL).HasConversion(
        v => JsonConvert.SerializeObject(v),
        v => JsonConvert.DeserializeObject<List<string>>(v));
            base.OnModelCreating(builder);
        }
    }
}
