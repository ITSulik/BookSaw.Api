using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookSaw.Seeder.Models;
using System.Text.Json;

namespace BookSaw.Seeder.Services;

internal sealed class ProductionSeeder(BookSawDbContext DbContext,
    ILogger<ProductionSeeder> logger) : ISeeder
{
    public const string Key = "ProductionSeeder";
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        
        await SeedCustomBooksAsync(cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCustomBooksAsync(CancellationToken cancellationToken)
{
    if (await DbContext.Books.AnyAsync(cancellationToken))
    {
        Logging.LoggerExtension.LogTableAlreadySeeded(logger, nameof(Book));
        return;
    }

    var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "books.json");
    if (!File.Exists(jsonPath))
    {
        logger.LogWarning("books.json not found at path: {Path}", jsonPath);
        return;
    }

    var jsonString = await File.ReadAllTextAsync(jsonPath, cancellationToken);
    var rawBooks = JsonSerializer.Deserialize<List<BookDto>>(jsonString, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }) ?? new List<BookDto>();

    var existingCategories = await DbContext.Categories.ToListAsync(cancellationToken);
    var categoryDict = existingCategories.ToDictionary(c => c.Name, c => c.Id);

    var newCategories = rawBooks
        .SelectMany(b => b.Categories)
        .Distinct()
        .Where(cat => !categoryDict.ContainsKey(cat))
        .Select(cat => new Category { Id = Guid.NewGuid(), Name = cat })
        .ToList();

    DbContext.Categories.AddRange(newCategories);
    await DbContext.SaveChangesAsync(cancellationToken);

    foreach (var newCat in newCategories)
    {
        categoryDict[newCat.Name] = newCat.Id;
    }

    var books = new List<Book>();
    var bookCategories = new List<BookCategory>();

    foreach (var raw in rawBooks)
    {
        var bookId = Guid.NewGuid();
        var book = new Book
        {
            Id = bookId,
            Title = raw.Title,
            Author = raw.Author,
            Description = raw.Description,
            Price = raw.Price,
            OldPrice = raw.OldPrice,
            InStock = raw.InStock,
            ImageUrl = raw.ImageUrl
        };

        foreach (var cat in raw.Categories)
        {
            if (categoryDict.TryGetValue(cat, out var catId))
            {
                bookCategories.Add(new BookCategory
                {
                    BookId = bookId,
                    CategoryId = catId
                });
            }
        }

        books.Add(book);
    }

    DbContext.Books.AddRange(books);
    DbContext.BookCategories.AddRange(bookCategories);
    Logging.LoggerExtension.LogTableSeeded(logger, nameof(Book), books.Count);
}
    
}