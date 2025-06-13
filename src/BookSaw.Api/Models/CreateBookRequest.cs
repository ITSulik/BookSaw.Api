using BookSaw.Api.Common.Interfaces.Services.BookService.Models;

namespace BookSaw.Api.Models.Requests;

public sealed record CreateBookRequest
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

public static class CreateBookRequestExtensions
{
    public static CreateBookCommand ToCommand(this CreateBookRequest request) => new CreateBookCommand
    (
        request.Title,
        request.Author,
        request.Description,
        request.Categories,
        request.Price,
        request.OldPrice,
        request.InStock,
        request.ImageUrl
    );
}