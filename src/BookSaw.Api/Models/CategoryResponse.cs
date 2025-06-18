using BookSaw.Api.Domain.Books;

namespace BookSaw.Api.Models.Responses;

public sealed record CategoryResponse
(
    Guid Id,
    string Name
)
{
    public static CategoryResponse FromDomainModel(Category category) => new CategoryResponse
    (
        category.Id,
        category.Name
    );
};
