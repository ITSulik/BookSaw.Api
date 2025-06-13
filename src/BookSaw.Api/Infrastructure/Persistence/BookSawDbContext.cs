using BookSaw.Api.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Api.Infrastructure.Persistence;

public class BookSawDbContext : DbContext
{
    public BookSawDbContext(DbContextOptions<BookSawDbContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookSawDbContext).Assembly);

        modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);

        base.OnModelCreating(modelBuilder);
    }
}
