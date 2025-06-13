namespace BookSaw.Api.Models.Requests;

public sealed record PatchBookRequest(
    string? Title = null,
    string? Author = null,
    string? Description = null,
    List<string>? Categories = null,
    decimal? Price = null,
    bool? InStock = null,
    string? ImageUrl = null
);