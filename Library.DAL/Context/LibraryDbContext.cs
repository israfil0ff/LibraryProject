using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Library.DAL.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<BookRental> BookRentals { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<History> Histories { get; set; }




        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (AuditableEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entity.ModifiedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entity.IsDeleted = true;
                        entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BookRental>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(LibraryDbContext)
                        .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : AuditableEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
