using DriveApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DriveApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<File> Files { get; set; }
        


        public override async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            var entries = ChangeTracker.Entries().Where(entry => entry.Entity is BaseEntity
            && entry.State == EntityState.Added
            || entry.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).Updated_at = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).Created_at = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Folder>()
                .Property(e => e.FolderId)
                .ValueGeneratedNever();
        }
    }


}
