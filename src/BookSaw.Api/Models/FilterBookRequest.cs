namespace BookSaw.Api.Models.Requests;

public sealed record FilterBooksRequest(
    string? Category,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy
);
