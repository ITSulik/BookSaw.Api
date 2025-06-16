using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;


namespace BookSaw.Seeder.Services;

internal sealed class ProductionSeeder(
    ILogger<ProductionSeeder> logger) : ISeeder
{
    public const string Key = "ProductionSeeder";
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        
        await Task.CompletedTask;
    }
}