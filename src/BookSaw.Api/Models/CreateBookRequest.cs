namespace BookSaw.Api.Models.Requests;

public sealed record CreateBookRequest
(
    string Title,
    string Author,
    string Description,
    List<string> Categories,
    decimal Price,
    bool InStock,
    string ImageUrl
);