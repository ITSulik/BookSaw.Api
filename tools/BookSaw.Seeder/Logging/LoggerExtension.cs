using Microsoft.Extensions.Logging;

namespace BookSaw.Seeder.Logging;

public static partial class LoggerExtension
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Seeding started...")]
    public static partial void LogSeedingStarted(this ILogger logger);


    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Seeding completed successfully.")]
    public static partial void LogSeedingCompleted(this ILogger logger);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "An error occurred during seeding")]
    public static partial void LogError(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Table {TableName} seeded with {Count} records.")]
    public static partial void LogTableSeeded(this ILogger logger, string tableName, int count);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Table {TableName} was previously seeded")]
    public static partial void LogTableAlreadySeeded(this ILogger logger, string tableName);


}