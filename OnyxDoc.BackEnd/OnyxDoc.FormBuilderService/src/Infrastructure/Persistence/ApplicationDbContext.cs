using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Domain.Common;
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
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Infrastructure.Utility;

namespace OnyxDoc.FormBuilderService.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    { 
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;

        public ApplicationDbContext(
            DbContextOptions options, 
            IDateTime dateTime) : base(options)
        {
          //  _currentUserService = currentUserService;
            _dateTime = dateTime;
        }


        #region Model      

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentPage> DocumentPages { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<ControlProperty> ControlProperties { get; set; }
        public DbSet<ControlPropertyItem> ControlPropertyItems { get; set; }

        public DbSet<PageControlItem> PageControlItems { get; set; }
        public DbSet<PageControlItemProperty> PageControlItemProperties { get; set; }
        public DbSet<PageControlItemPropertyValue> PageControlItemPropertyValues { get; set; }
        public DbSet<Sequence> Sequences { get; set; }
        public DbSet<DocumentSequence> DocumentSequences { get; set; }
        public DbSet<DocumentReminder> DocumentReminders { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            #region indexing

            modelBuilder.Entity<ControlProperty>()
                .AddIndex(b => b.HasIndex(x => x.ControlId))
                .AddIndex(b => b.HasIndex(x => x.Index));

            modelBuilder.Entity<ControlPropertyItem>()
               .AddIndex(b => b.HasIndex(x => x.ControlPropertyId))
               .AddIndex(b => b.HasIndex(x => x.Index))
               .AddIndex(b => b.HasIndex(x => x.Value));

            modelBuilder.Entity<DocumentPage>()
              .AddIndex(b => b.HasIndex(x => x.DocumentId))
              .AddIndex(b => b.HasIndex(x => x.PageIndex))
              .AddIndex(b => b.HasIndex(x => x.PageNumber));
            #endregion

            //modelBuilder.Entity<ControlProperty>(p =>
            //    {
            //        p.HasOne(e => e.ParentControlProperty)
            //        .WithMany()
            //        .OnDelete(DeleteBehavior.NoAction); 
            //    });              


            modelBuilder.Entity<Control>()
                .Property(p => p.VersionNumber)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Document>()
               .Property(p => p.VersionNumber)
               .HasColumnType("decimal(18,2)");
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
