using Microsoft.Extensions.Logging;

namespace BookSaw.Seeder.Logging;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Seeding started")]
    public static partial void LogSeedingStarted(this ILogger logger);
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Seeding completed")]
    public static partial void LogSeedingCompleted(this ILogger logger);
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Table '{Table}' was seeded with '{Records}' records")]
    public static partial void LogTableSeeded(this ILogger logger, string table, int records);
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Table '{Table}' was previously seeded")]
    public static partial void LogTablePreviouslySeeded(this ILogger logger, string table);
}
