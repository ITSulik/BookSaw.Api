using BookSaw.Api.Common.Interfaces.Services.BookService.Models;

namespace BookSaw.Api.Models.Requests;

public sealed record UpdateBookRequest(
    string Title,
    string Author,
    string Description,
    List<string> Categories,
    decimal Price,
    decimal? OldPrice,
    bool InStock,
    string ImageUrl
);

public static class UpdateBookRequestExtensions
{
    public static UpdateBookCommand ToCommand(this UpdateBookRequest request) => new UpdateBookCommand
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
