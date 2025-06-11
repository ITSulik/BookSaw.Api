using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;

namespace BookSaw.Api.Common.Interfaces.Services;

public interface IBookService
{
    Task<BookResponse> CreateAsync(CreateBookRequest request);
    Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request);
    Task<BookResponse?> GetByIdAsync(Guid id);
    Task<List<BookResponse>> GetAllAsync();
    Task DeleteAsync(Guid id);
}