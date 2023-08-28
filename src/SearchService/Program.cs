using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

await DbInitializer.InitDb(app);

app.UseAuthorization();

app.MapControllers();

app.Run();
