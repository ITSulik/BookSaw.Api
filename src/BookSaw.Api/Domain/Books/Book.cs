namespace BookSaw.Api.Domain.Books;

public sealed class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

    public decimal Price { get; set; }

    public decimal? OldPrice { get; set; }

    public bool InStock { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    
    public Book()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
