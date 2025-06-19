using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Reflection.Metadata;
using BookSaw.Seeder.Models;

namespace BookSaw.Seeder.Services;

internal sealed class StagingSeeder(BookSawDbContext dbContext,
    ILogger<StagingSeeder> logger) : ISeeder
{
    public const string Key = "StagingSeeder";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        Logging.LoggerExtension.LogSeedingStarted(logger);

        // await SeedCategoriesAsync(cancellationToken);
        // await dbContext.SaveChangesAsync(cancellationToken);
        // await SeedBooksAsync(cancellationToken);
        // await dbContext.SaveChangesAsync(cancellationToken);

        await SeedCustomBooksAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logging.LoggerExtension.LogSeedingCompleted(logger);
    }

    private async Task SeedBooksAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Books.AnyAsync(cancellationToken))
        {
            Logging.LoggerExtension.LogTableAlreadySeeded(logger, nameof(Book));
            return;
        }
        var categories = await dbContext.Categories.ToListAsync(cancellationToken);

        var faker = new Faker<Book>().UseSeed(10).CustomInstantiator(f =>
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Author = f.Person.FullName,
                Title = f.Commerce.ProductName(),
                Description = f.Commerce.ProductDescription(),
                Price = f.Finance.Amount(min: 0, max: 100, decimals: 2),
                InStock = f.Random.Bool(),
                ImageUrl = f.Image.PicsumUrl(width: 220, height: 320),
                
            };

            if (f.Random.Bool())
            {
                book.OldPrice = book.Price + f.Finance.Amount(min: 1, max: 20, decimals: 2);
            }

            var categories2 = f.PickRandom(categories, f.Random.Int(1, 4)).ToList();
            foreach (var c in categories2)
            {
                dbContext.BookCategories.Add(new BookCategory
                {
                    CategoryId = c.Id,
                    BookId = book.Id
                });
            }

            return book;

        });

        var books = faker.Generate(50);
        dbContext.Books.AddRange(books);
        Logging.LoggerExtension.LogTableSeeded(logger, nameof(Book), books.Count);
    }

    private async Task SeedCategoriesAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Categories.AnyAsync(cancellationToken))
        {
            Logging.LoggerExtension.LogTableAlreadySeeded(logger, nameof(Category));
            return;
        }

        var faker = new Faker<Category>().UseSeed(10).CustomInstantiator(f =>
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = f.Commerce.Categories(1).First()
            };

            return category;
        });

        var categories = faker.Generate(10);
        dbContext.Categories.AddRange(categories);
        Logging.LoggerExtension.LogTableSeeded(logger, nameof(Category), categories.Count);
    }

    

    private async Task SeedCustomBooksAsync(CancellationToken cancellationToken)
{
    if (await dbContext.Books.AnyAsync(cancellationToken))
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

    var existingCategories = await dbContext.Categories.ToListAsync(cancellationToken);
    var categoryDict = existingCategories.ToDictionary(c => c.Name, c => c.Id);

    var newCategories = rawBooks
        .SelectMany(b => b.Categories)
        .Distinct()
        .Where(cat => !categoryDict.ContainsKey(cat))
        .Select(cat => new Category { Id = Guid.NewGuid(), Name = cat })
        .ToList();

    dbContext.Categories.AddRange(newCategories);
    await dbContext.SaveChangesAsync(cancellationToken);

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

    dbContext.Books.AddRange(books);
    dbContext.BookCategories.AddRange(bookCategories);
    Logging.LoggerExtension.LogTableSeeded(logger, nameof(Book), books.Count);
}
}