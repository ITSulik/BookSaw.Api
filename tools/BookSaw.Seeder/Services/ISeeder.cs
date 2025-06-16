namespace BookSaw.Seeder.Services;

internal interface ISeeder
{
        Task SeedAsync( CancellationToken cancellationToken = default);
}