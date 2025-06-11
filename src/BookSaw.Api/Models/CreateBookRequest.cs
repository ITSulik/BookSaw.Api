namespace BookSaw.Api.Models.Requests;

public class CreateBookRequest
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Categories { get; set; } = new();
    public decimal Price { get; set; }
    public bool InStock { get; set; }
    public string ImageUrl { get; set; } = null!;
}