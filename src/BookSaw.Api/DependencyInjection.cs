using BookSaw.Api.Services;
using BookSaw.Api.Common.Interfaces.Services.BookService;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookSaw.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookSawDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BookSawDb")));

        return services;
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();

        return services;
    }
}