using Bogus;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookSaw.Seeder.Services;

internal sealed class StagingSeeder(
    BookSawDbContext dbContext,
    ILogger<StagingSeeder> logger) : ISeeder
{
    public const string Key = "staging";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        Logging.LoggerExtensions.LogSeedingStarted(logger);
        
        await SeedCategoriesAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await SeedBooksAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        Logging.LoggerExtensions.LogSeedingCompleted(logger);
    }

    private async Task SeedBooksAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Books.AnyAsync(cancellationToken))
        {
            //LoggerExtensions.LogTablePreviouslySeeded(logger, nameof(dbContext.Payments));
            return;
        }
        
        var categories = await dbContext.Categories.ToListAsync(cancellationToken);

        var faker = new Faker<Book>().UseSeed(11).CustomInstantiator(f =>
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Author = f.Person.FullName,
                Title = f.Commerce.ProductName(),
                Description = f.Commerce.ProductDescription(),
                Price = f.Finance.Amount(min: 0, max: 100, decimals: 2),
                InStock = f.Random.Bool(),
                ImageUrl = f.Image.PicsumUrl(width: 100, height: 100),
                CreatedAt = DateTime.Now
            };

            if (f.Random.Bool())
            {
                book.OldPrice = f.Finance.Amount(min: 0, max: 100, decimals: 2);
            }

            var category = f.PickRandom(categories);
            book.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = category.Id,
                Category = category,
                Book = book
            });

            return book;
        });
        
        var books = faker.Generate(2000);
        dbContext.Books.AddRange(books);
        Logging.LoggerExtensions.LogTableSeeded(logger, nameof(dbContext.Books), books.Count);
    }

    private async Task SeedCategoriesAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Categories.AnyAsync(cancellationToken))
        {
            Logging.LoggerExtensions.LogTablePreviouslySeeded(logger, nameof(dbContext.Categories));
            return;
        }
        
        var faker = new Faker<Category>().UseSeed(11).CustomInstantiator(f =>
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
        Logging.LoggerExtensions.LogTableSeeded(logger, nameof(dbContext.Categories), categories.Count);
    }
}