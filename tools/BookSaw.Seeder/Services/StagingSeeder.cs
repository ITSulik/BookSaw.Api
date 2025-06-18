using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Seeder.Services;

internal sealed class StagingSeeder(BookSawDbContext dbContext,
    ILogger<StagingSeeder> logger) : ISeeder
{
    public const string Key = "StagingSeeder";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        Logging.LoggerExtension.LogSeedingStarted(logger);

        await SeedCategoriesAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await SeedBooksAsync(cancellationToken);
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
}