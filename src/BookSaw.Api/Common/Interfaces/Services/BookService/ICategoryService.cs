using BookSaw.Api.Domain.Books;

namespace BookSaw.Api.Common.Interfaces.Services.BookService;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoriesAsync();
}