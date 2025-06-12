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

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is not null) 
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    
}
