using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;
using BookSaw.Api.Domain.Books;

namespace BookSaw.Api.Common.Interfaces.Services;

public interface IBookService
{
    Task<Book> CreateAsync(CreateBookRequest request);
    Task<Book> UpdateAsync(Guid id, UpdateBookRequest request);
    Task<Book> GetByIdAsync(Guid id);
    Task<List<Book>> GetAllAsync();
    Task DeleteAsync(Guid id);
}