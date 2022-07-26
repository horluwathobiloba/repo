using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using OnyxDoc.FormBuilderService.Domain.Entities;

namespace OnyxDoc.FormBuilderService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        DbSet<Document> Documents { get; set; }
        DbSet<DocumentPage> DocumentPages { get; set; }
        DbSet<Control> Controls { get; set; }
        DbSet<ControlProperty> ControlProperties { get; set; }
        DbSet<ControlPropertyItem> ControlPropertyItems { get; set; }

        DbSet<PageControlItem> PageControlItems { get; set; }
        DbSet<PageControlItemProperty> PageControlItemProperties { get; set; }
        DbSet<PageControlItemPropertyValue> PageControlItemPropertyValues { get; set; }

        DbSet<Sequence> Sequences { get; set; }
        DbSet<DocumentSequence> DocumentSequences { get; set; }
        DbSet<DocumentReminder> DocumentReminders { get; set; }


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
