using BookSaw.Api.Common.Interfaces.Services.BookService;
using BookSaw.Api.Common.Exceptions;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookSaw.Api.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _catService = categoryService;
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll()
    {
        var categories = await _catService.GetAllCategoriesAsync();
        return Ok(categories.Select(CategoryResponse.FromDomainModel).ToList());
    }

}