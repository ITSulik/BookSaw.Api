using BookSaw.Api;
using BookSaw.Seeder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookSaw.Seeder;

public static class DependencyInjection
{
    public static IServiceCollection AddSeederServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataBase(configuration);
        services.AddScoped<MigrationApplier>();
        services.AddKeyedScoped<ISeeder, StagingSeeder>(StagingSeeder.Key);
        services.AddKeyedScoped<ISeeder, ProductionSeeder>(ProductionSeeder.Key);

        return services;
    }
}