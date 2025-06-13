using BookSaw.Api.Common.Interfaces.Services.BookService;
using BookSaw.Api.Common.Exceptions;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Models.Requests;
using BookSaw.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookSaw.Api.Controllers;

[ApiController]
[Route("books")]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;
    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAll()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books.Select(BookResponse.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetById(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(BookResponse.FromDomainModel(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request)
    {
        var book = await _bookService.AddBookAsync(request.ToCommand());
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, BookResponse.FromDomainModel(book));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookRequest request)
    {
        var book = await _bookService.UpdateBookAsync(id, request.ToCommand());
        if (book is null)
        {
            return NotFound();
        }
        return Ok(BookResponse.FromDomainModel(book));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        await _bookService.DeleteBookAsync(id);

        return NoContent();
    }


    [HttpGet("search")]
    public async Task<ActionResult<List<BookResponse>>> Search([FromQuery] string? query)
    {
        var books = await _bookService.SearchAsync(query ?? string.Empty);
        if (books is null || !books.Any())
        {
            return NotFound("No books found matching the search criteria.");
        }
        return Ok(books.Select(BookResponse.FromDomainModel).ToList());
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<BookResponse>>> Filter([FromQuery] FilterBooksRequest filter)
    {
        var books = await _bookService.FilterAsync(filter.ToCommand());
        if (books is null || !books.Any())
        {
            return NotFound("No books found matching the filter criteria.");
        }
        return Ok(books.Select(BookResponse.FromDomainModel).ToList());
    }


}