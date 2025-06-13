namespace BookSaw.Api.Domain.Books;

public sealed class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();


}