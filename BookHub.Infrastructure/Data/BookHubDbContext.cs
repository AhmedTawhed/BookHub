using BookHub.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure.Data
{
    public class BookHubDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookHubDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite Key for FavoriteBook
            builder.Entity<FavoriteBook>()
                .HasKey(fb => new { fb.UserId, fb.BookId });

            builder.Entity<FavoriteBook>()
                .HasOne(fb => fb.User)
                .WithMany()
                .HasForeignKey(fb => fb.UserId);

            builder.Entity<FavoriteBook>()
                .HasOne(fb => fb.Book)
                .WithMany(b => b.FavoriteUsers)
                .HasForeignKey(fb => fb.BookId);

            // Review relationships
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}