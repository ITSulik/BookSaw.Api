using BookSaw.Api.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Api.Infrastructure.Persistence;

public class BookSawDbContext : DbContext
{
    public BookSawDbContext(DbContextOptions<BookSawDbContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookSawDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
