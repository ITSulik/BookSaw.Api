namespace BookSaw.Api.Common.Interfaces.Services.BookService.Models;

public sealed record FilterBooksCommand(
    string? Category,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy
);