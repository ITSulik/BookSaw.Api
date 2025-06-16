using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookSaw.Seeder.Services;

internal sealed class MigrationApplier(
    ILogger<MigrationApplier> logger, BookSawDbContext dbContext)
{
    public async Task ApplyAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Initializing database connection...");
        
        if (!await IsDbReadyAsync(retryCount: 10, delaySeconds: 5, cancellationToken: cancellationToken))
        {
            logger.LogError("Database connection could not be established after multiple attempts.");
            throw new Exception("Database is unavailable.");
        }
        
        logger.LogDebug("Database connection established.");
        
        if((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToList() is not { Count: > 0 } migrations)
        {
            return;
        }
        
        migrations.ForEach(migration => logger.LogDebug("Applying migration: {migration}", migration));
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
    
    private async Task<bool> IsDbReadyAsync(
        int retryCount = 10, 
        int delaySeconds = 5, 
        CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; attempt <= retryCount; attempt++)
        {
            if (await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return true;
            }
            
            logger.LogDebug("Database not ready. Waiting {DelaySeconds} seconds...", delaySeconds);
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
        }
        
        return false;
    }
}