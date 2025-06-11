using BookSaw.Api.Services;
using BookSaw.Api.Common.Interfaces.Services;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookSawDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BookSawDb")));

        services.AddScoped<IBookService, BookService>();

        return services;
    }

    public static IHostBuilder UseApiServices(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddDataBase(context.Configuration);
        });
    }
}