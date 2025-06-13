using BookSaw.Api.Common.Interfaces.Services.BookService.Models;

namespace BookSaw.Api.Models.Requests;

public sealed record FilterBooksRequest(
    string? Category,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy
);

public static class FilterBooksRequestExtensions
{
    public static FilterBooksCommand ToCommand(this FilterBooksRequest request) => new FilterBooksCommand
    (
        request.Category,
        request.MinPrice,
        request.MaxPrice,
        request.SortBy
    );
}