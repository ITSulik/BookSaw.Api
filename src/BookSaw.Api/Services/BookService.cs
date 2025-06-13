using BookSaw.Api.Common.Interfaces.Services;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;
using BookSaw.Api.Common.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace BookSaw.Api.Services;

public class BookService(BookSawDbContext context) : IBookService
{
    private readonly BookSawDbContext _context = context;


    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
        {
            throw new NotFoundException($"Cartea nu a fost găsită.");
        }

        return book;
    }

    public async Task<Book> CreateAsync(CreateBookRequest request)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            Description = request.Description,
            Categories = request.Categories,
            Price = request.Price,
            InStock = request.InStock,
            CreatedAt = DateTime.UtcNow,
            ImageUrl = request.ImageUrl
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<Book> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        var book = await _context.Books.FirstOrDefaultAsync(u => u.Id == id);
        if (book is null)
        {
            throw new NotFoundException($"Cartea nu a fost găsită.");
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.Description = request.Description;
        book.Categories = request.Categories;
        book.Price = request.Price;
        book.InStock = request.InStock;
        book.ImageUrl = request.ImageUrl;

        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<Book> PatchAsync(Guid id, PatchBookRequest request)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
        {
            throw new NotFoundException("Cartea nu a fost găsită.");
        }

        if (request.Title is not null) book.Title = request.Title;
        if (request.Author is not null) book.Author = request.Author;
        if (request.Description is not null) book.Description = request.Description;
        if (request.Categories is not null) book.Categories = request.Categories;
        if (request.Price.HasValue) book.Price = request.Price.Value;
        if (request.InStock.HasValue) book.InStock = request.InStock.Value;
        if (request.ImageUrl is not null) book.ImageUrl = request.ImageUrl;

        await _context.SaveChangesAsync();

        return book;
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is not null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Book>> FilterAsync(FilterBooksRequest filter)
{
    var query = _context.Books.AsQueryable();

    if (filter.MinPrice.HasValue)
        query = query.Where(b => b.Price >= filter.MinPrice.Value);

    if (filter.MaxPrice.HasValue)
        query = query.Where(b => b.Price <= filter.MaxPrice.Value);

    var result = await query.ToListAsync(); // execuți în DB doar ce poate fi tradus

    if (!string.IsNullOrWhiteSpace(filter.Category))
        result = result.Where(b => b.Categories.Any(c => c.Equals(filter.Category, StringComparison.OrdinalIgnoreCase))).ToList();

    result = filter.SortBy switch
    {
        "price_asc" => result.OrderBy(b => b.Price).ToList(),
        "price_desc" => result.OrderByDescending(b => b.Price).ToList(),
        "title_asc" => result.OrderBy(b => b.Title).ToList(),
        "title_desc" => result.OrderByDescending(b => b.Title).ToList(),
        _ => result.OrderByDescending(b => b.CreatedAt).ToList()
    };

    return result;
}


    public async Task<List<Book>> SearchAsync(string? searchTerm)
{
    if (string.IsNullOrWhiteSpace(searchTerm))
    {
        return await _context.Books
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    searchTerm = searchTerm.ToLower();

    // Pas 1: cărți din SQL care conțin în titlu, autor sau descriere
    var matchedFromDb = await _context.Books
        .Where(b =>
            b.Title.ToLower().Contains(searchTerm) ||
            b.Author.ToLower().Contains(searchTerm) ||
            b.Description.ToLower().Contains(searchTerm))
        .ToListAsync();

    // Pas 2: cărți din toate care conțin în categories (în memorie)
    var allBooks = await _context.Books.ToListAsync();
    var matchedFromCategories = allBooks
        .Where(b => b.Categories.Any(c => c.ToLower().Contains(searchTerm)))
        .ToList();

    // Pas 3: combină ambele seturi, elimină duplicatele
    var allMatched = matchedFromDb
        .Union(matchedFromCategories)
        .OrderByDescending(b => b.CreatedAt)
        .ToList();

    return allMatched;
}






}
