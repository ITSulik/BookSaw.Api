using BookSaw.Api.Common.Interfaces.Services.BookService;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;
using BookSaw.Api.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookSaw.Api.Common.Interfaces.Services.BookService.Models;


namespace BookSaw.Api.Services;

public class BookService(BookSawDbContext context) : IBookService
{
    private readonly BookSawDbContext _context = context;


    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books
         .Include(b => b.BookCategories).ThenInclude(bc => bc.Category).ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        var book = await _context.Books.Include(b => b.BookCategories).ThenInclude(bc => bc.Category).FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
        {
            throw new NotFoundException($"Cartea nu a fost găsită.");
        }
        return book;
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is not null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }




    public async Task<List<Book>> SearchAsync(string? searchTerm)
{
    if (string.IsNullOrWhiteSpace(searchTerm))
    {
        return await _context.Books
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    searchTerm = searchTerm.ToLower();

    return await _context.Books
        .Include(b => b.BookCategories)
        .ThenInclude(bc => bc.Category)
        .Where(b =>
            b.Title.ToLower().Contains(searchTerm) ||
            b.Author.ToLower().Contains(searchTerm) ||
            b.Description.ToLower().Contains(searchTerm) ||
            b.BookCategories.Any(bc => bc.Category.Name.ToLower().Contains(searchTerm))
        )
        .OrderByDescending(b => b.CreatedAt)
        .ToListAsync();
}

    public async Task<Book> AddBookAsync(CreateBookCommand request)
    {
    var book = new Book
    {
        Id = Guid.NewGuid(),
        Title = request.Title,
        Author = request.Author,
        Description = request.Description,
        BookCategories = new List<BookCategory>(),
        Price = request.Price,
        OldPrice = request.OldPrice,
        InStock = request.InStock,
        ImageUrl = request.ImageUrl
    };
    
    _context.Books.Add(book);
    await _context.SaveChangesAsync();

    foreach (var categoryName in request.Categories.Distinct())
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                category = new Category { Id = Guid.NewGuid(), Name = categoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            book.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = category.Id,
                Book = book,
                Category = category
            });
        }

    
    await _context.SaveChangesAsync();

    return book;
}


    public async Task<Book> UpdateBookAsync(Guid id, UpdateBookCommand request)
{
    var book = await _context.Books
        .Include(b => b.BookCategories)
        .ThenInclude(bc => bc.Category)
        .FirstOrDefaultAsync(b => b.Id == id);

    if (book is null)
    {
        throw new NotFoundException($"Cartea nu a fost găsită.");
    }

    // Update basic properties
    book.Title = request.Title;
    book.Author = request.Author;
    book.Description = request.Description;
    book.Price = request.Price;
    book.OldPrice = request.OldPrice;
    book.InStock = request.InStock;
    book.ImageUrl = request.ImageUrl;

    // Actualizează categoriile
    book.BookCategories.Clear();

    foreach (var categoryName in request.Categories.Distinct())
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

        if (category is null)
        {
            category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryName
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        book.BookCategories.Add(new BookCategory
        {
            BookId = book.Id,
            CategoryId = category.Id,
            Category = category,
            Book = book
        });
    }

    await _context.SaveChangesAsync();
    return book;
}



    public async Task<List<Book>> FilterAsync(FilterBooksCommand filter)
{
    var query = _context.Books
        .Include(b => b.BookCategories)
        .ThenInclude(bc => bc.Category)
        .AsQueryable();

    if (filter.MinPrice.HasValue)
        query = query.Where(b => b.Price >= filter.MinPrice.Value);

    if (filter.MaxPrice.HasValue)
        query = query.Where(b => b.Price <= filter.MaxPrice.Value);

    if (!string.IsNullOrWhiteSpace(filter.Category))
    {
        var categoryLower = filter.Category.ToLower();
        query = query.Where(b =>
            b.BookCategories.Any(bc =>
                bc.Category.Name.ToLower() == categoryLower
            ));
    }

    query = filter.SortBy switch
    {
        "price_asc" => query.OrderBy(b => b.Price),
        "price_desc" => query.OrderByDescending(b => b.Price),
        "title_asc" => query.OrderBy(b => b.Title),
        "title_desc" => query.OrderByDescending(b => b.Title),
        _ => query.OrderByDescending(b => b.CreatedAt)
    };

    return await query.ToListAsync();
}

}
