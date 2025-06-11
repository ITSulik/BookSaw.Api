using BookSaw.Api.Common.Interfaces.Services;
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
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request)
    {
        var book = await _bookService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookRequest request)
    {
        var book = await _bookService.UpdateAsync(id, request);
        if (book is null)
        {
            return NotFound();
        }
        return Ok(book);
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
}