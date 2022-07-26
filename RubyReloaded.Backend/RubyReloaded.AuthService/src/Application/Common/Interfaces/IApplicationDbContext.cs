using RubyReloaded.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Client> Clients { get; set; }
        DbSet<PermissionSet> PermissionSets { get; set; }
        DbSet<CooperativeUserMapping> CooperativeMembers { get; set; }
        DbSet<UserLinkInvite> UserLinkCreations { get; set; }
        DbSet<UserCount> UserCount { get; set; }
        DbSet<Cooperative> Cooperatives { get; set; }
        DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        DbSet<CooperativeRole> Roles{ get; set; }
        DbSet<Domain.Entities.AjoRole> AjoRoles{ get; set; }
        DbSet<SystemOwnerRole> SystemOwnerRoles{ get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CooperativeRolePermission> RolePermissions { get; set; }
        DbSet<Domain.Entities.AjoRolePermission> AjoRolePermissions { get; set; }
        DbSet<Domain.Entities.SystemOwnerRolePermission> SystemOwnerRolePermissions { get; set; }
        DbSet<Domain.Entities.SystemOwnerUsers> SystemOwnerUsers { get; set; }
        DbSet<Domain.Entities.CurrencyConfiguration> CurrencyConfigurations { get; set; }

        DbSet<CooperativeSettings> CooperativeSettings { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<Ajo> Ajos { get; set; }
        DbSet<AjoMember> AjoMembers{ get; set; }
        DbSet<AjoCode> AjoCodes{ get; set; }
        DbSet<CooperativeUserCode> CooperativeUserCodes{ get; set; }
        DbSet<RequestToJoinTracker> RequestToJoinTrackers{ get; set; }
        DbSet<AjoInvitationTracker> AjoInvitationTrackers{ get; set; }
        DbSet<CooperativeInvitationTracker> CooperativeInvitationTrackers{ get; set; }
        DbSet<Domain.Entities.SystemOwner> SystemOwners{ get; set; }
        DbSet<Domain.Entities.PaymentChannel> PaymentChannels{ get; set; }
        DbSet<Domain.Entities.FAQCategory> FAQCategories{ get; set; }
        DbSet<Domain.Entities.FAQQuestion> FAQQuestions{ get; set; }
        DbSet<Domain.Entities.Tag> Tags{ get; set; }
        DbSet<Domain.Entities.TagFAQ> TagFAQs{ get; set; }
        DbSet<Domain.Entities.ExploreCategory> ExploreCategories{ get; set; }
        DbSet<Domain.Entities.ExplorePost> ExplorePosts{ get; set; }
        DbSet<Domain.Entities.ExplorePostFile> ExplorePostFiles{ get; set; }
     
        DbSet<Domain.Entities.ExploreTag> ExploreTags{ get; set; }
        DbSet<Domain.Entities.ExploreTagPost> ExploreTagPosts{ get; set; }
       

        
        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void RollbackTransaction();
    }
}
