using BookSaw.Api.Common.Interfaces.Services.BookService.Models;
using BookSaw.Api.Domain.Books;

namespace BookSaw.Api.Common.Interfaces.Services.BookService;

public interface IBookService
{
    Task<Book> AddBookAsync(CreateBookCommand request);

    Task<Book> UpdateBookAsync(Guid id, UpdateBookCommand request);

    Task<Book> GetBookByIdAsync(Guid id);

    Task<List<Book>> GetAllBooksAsync();

    Task DeleteBookAsync(Guid id);

    Task<List<Book>> SearchAsync(string query);

    Task<List<Book>> FilterAsync(FilterBooksCommand filter);
}