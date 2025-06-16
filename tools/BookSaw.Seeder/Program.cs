using BookSaw.Seeder;
using BookSaw.Seeder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable UnusedVariable

#region AppConfiguration

var host = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, config) =>
               {
                   var env = context.HostingEnvironment;
                   config.SetBasePath(Directory.GetCurrentDirectory());
                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                         .AddUserSecrets<Program>(optional: true);
               })
               .ConfigureServices((context, services) =>
               {
                   services.AddSeederServices(context.Configuration);
               })
               .Build();

#endregion

using var scope = host.Services.CreateScope();
var scopedProvider = scope.ServiceProvider;

var environment = scopedProvider.GetRequiredService<IHostEnvironment>();
var seederKey = environment.IsProduction() ? 
    ProductionSeeder.Key : 
    StagingSeeder.Key;

var migrator = scopedProvider.GetRequiredService<MigrationApplier>();
await migrator.ApplyAsync();

var seeder = scopedProvider.GetRequiredKeyedService<ISeeder>(seederKey);
await seeder.SeedAsync();