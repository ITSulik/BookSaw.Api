namespace BookSaw.Seeder.Models;

public sealed class BookDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Categories { get; set; } = new List<string>();

    public decimal Price { get; set; }

    public decimal? OldPrice { get; set; }

    public bool InStock { get; set; }


    public string ImageUrl { get; set; } = string.Empty;
    
}