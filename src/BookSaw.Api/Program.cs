using BookSaw.Api;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services
           .AddDataBase(builder.Configuration)
           .AddApplicationServices(); 
}

var app = builder.Build();
{
    app.MapControllers();
}

app.Run();

