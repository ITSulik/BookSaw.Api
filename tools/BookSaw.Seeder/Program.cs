using BookSaw.Seeder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using BookSaw.Seeder.Services;
using Microsoft.Extensions.DependencyInjection;


#region AppConfiguration

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

    }).ConfigureServices((context, services) =>
    {
        services.AddSeederServices(context.Configuration);
    })
    .Build();

#endregion

using var scope = host.Services.CreateScope();
var scopedprovider = scope.ServiceProvider;

var environment = scopedprovider.GetRequiredService<IHostEnvironment>();
var seederkey = environment.IsDevelopment() ? ProductionSeeder.Key : StagingSeeder.Key;

var migrationApplier = scopedprovider.GetRequiredService<MigrationApplier>();
await migrationApplier.ApplyAsync();

var seeder = scopedprovider.GetRequiredKeyedService<ISeeder>("StagingSeeder");
await seeder.SeedAsync();