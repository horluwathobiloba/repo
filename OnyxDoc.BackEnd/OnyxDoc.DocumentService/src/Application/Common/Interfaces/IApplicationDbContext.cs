using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using OnyxDoc.DocumentService.Domain.Entities;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Component> Components { get; set; }
        DbSet<Domain.Entities.Coordinate> Coordinates { get; set; }
        DbSet<Domain.Entities.Document> Documents { get; set; }
        DbSet<Domain.Entities.DocumentFeedback> DocumentFeedbacks { get; set; }
        DbSet<Domain.Entities.ContactFeedback> ContactFeedbacks { get; set; }

        DbSet<Domain.Entities.Recipient> Recipients { get; set; }
        DbSet<Domain.Entities.RecipientAction> RecipientActions { get; set; }
        DbSet<Domain.Entities.Comment> Comments { get; set; }
        DbSet<Domain.Entities.AddressBook> AddressBooks { get; set; }
      
        DbSet<Domain.Entities.Inbox> Inboxes { get; set; }
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<Domain.Entities.Folder> Folders { get; set; }
        DbSet<FolderShareDetail> FolderShareDetails { get; set; }
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
