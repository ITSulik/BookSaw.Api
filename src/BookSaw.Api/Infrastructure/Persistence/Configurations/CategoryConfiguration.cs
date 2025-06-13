using BookSaw.Api.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSaw.Api.Infrastructure.Persistence.Configurations;

[Table("Categories")]
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder
            .HasMany(c => c.BookCategories)
            .WithOne(bc => bc.Category)
            .HasForeignKey(bc => bc.CategoryId);
    }
}
