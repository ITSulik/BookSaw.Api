

namespace BookSaw.Api.Domain.Books;

public class BookCategory
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}