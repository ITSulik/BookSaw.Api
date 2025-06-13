using BookSaw.Api.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSaw.Api.Infrastructure.Persistence.Configurations;

[Table("Books")]
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Description).IsRequired();

        builder.Property(b => b.Price).HasColumnType("decimal(10,2)");
        builder.Property(b => b.OldPrice).HasColumnType("decimal(10,2)").IsRequired(false);
        builder.Property(b => b.InStock).IsRequired();
        builder.Property(b => b.ImageUrl).IsRequired();

        builder
        .HasMany(b => b.BookCategories)
        .WithOne(bc => bc.Book)
        .HasForeignKey(bc => bc.BookId);
    }
}
