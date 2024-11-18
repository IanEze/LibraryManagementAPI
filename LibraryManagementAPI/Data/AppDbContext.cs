using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }

        // Fluent API to configure relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          

            // Book and Author relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author) // Each book has one author
                .WithMany(a => a.Books) // Each author can write many books
                .HasForeignKey(b => b.AuthorId) // Foreign key in the Book table
                .OnDelete(DeleteBehavior.Cascade); // Deleting an author deletes their books
        }
    }
}
