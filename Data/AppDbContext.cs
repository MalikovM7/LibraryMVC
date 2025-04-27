using LibraryMVC.Models.Enums;
using LibraryMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Status)
                      .HasDefaultValue(Status.Active);
            });

           
            modelBuilder.Entity<Reader>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Status)
                      .HasDefaultValue(Status.Active);
            });

            
            modelBuilder.Entity<Borrow>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Status)
                      .HasDefaultValue(BorrowStatus.Borrowed);

                entity.HasOne(b => b.Book)
                      .WithMany()
                      .HasForeignKey(b => b.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Reader)
                      .WithMany()
                      .HasForeignKey(b => b.ReaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
