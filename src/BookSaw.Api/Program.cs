using BookSaw.Api.Services;
using BookSaw.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookSaw.Api.Common.Interfaces.Services;
using BookSaw.Api;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddDataBase(builder.Configuration); 
}

var app = builder.Build();
    {
        app.MapControllers();
    }

app.Run();

