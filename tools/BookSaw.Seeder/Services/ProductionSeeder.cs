using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;
using BookSaw.Api.Domain.Books;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookSaw.Seeder.Models;
using System.Text.Json;

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