using BookSaw.Api.Common.Interfaces.Services;
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
        var books = await _bookService.GetAllAsync();
        return Ok(books.Select(BookResponse.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(BookResponse.FromDomainModel(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request)
    {
        var book = await _bookService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, BookResponse.FromDomainModel(book));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookRequest request)
    {
        var book = await _bookService.UpdateAsync(id, request);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(BookResponse.FromDomainModel(book));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        await _bookService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]    
    public async Task<ActionResult<BookResponse>> Patch(Guid id, PatchBookRequest request)
    {
    try
    {
        var book = await _bookService.PatchAsync(id, request);
        return Ok(book);
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

}