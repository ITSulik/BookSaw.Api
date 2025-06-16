using Microsoft.Extensions.Logging;

namespace BookSaw.Seeder.Services;

internal sealed class ProductionSeeder(
    ILogger<ProductionSeeder> logger) : ISeeder
{
    public const string Key = "production";
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}