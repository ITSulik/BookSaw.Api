using BookSaw.Api.Common.Interfaces.Services.BookService;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;
using BookSaw.Api.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Api.Services;

public class CategoryService(BookSawDbContext context) : ICategoryService
{
    private readonly BookSawDbContext _context = context;
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}