using BookSaw.Api.Domain.Books;

namespace BookSaw.Api.Models.Responses;

public sealed record BookResponse
(
    Guid Id,
    string Title,
    string Author,
    string Description,
    List<string> Categories,
    decimal Price,
    decimal? OldPrice,
    bool InStock,
    DateTime CreatedAt,
    string ImageUrl
)
{
    public static BookResponse FromDomainModel(Book book) => new BookResponse
    (
        book.Id,
        book.Title,
        book.Author,
        book.Description,
        book.BookCategories.Select(bc => bc.Category.Name).ToList(),
        book.Price,
        book.OldPrice,
        book.InStock,
        book.CreatedAt,
        book.ImageUrl
    );
}