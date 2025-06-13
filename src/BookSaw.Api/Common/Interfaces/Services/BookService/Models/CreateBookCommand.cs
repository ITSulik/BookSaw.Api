namespace BookSaw.Api.Common.Interfaces.Services.BookService.Models;

public sealed record CreateBookCommand
(
    string Title,
    string Author,
    string Description,
    List<string> Categories,
    decimal Price,
    decimal? OldPrice,
    bool InStock,
    string ImageUrl
);