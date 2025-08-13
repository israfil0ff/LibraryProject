using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookRental>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
