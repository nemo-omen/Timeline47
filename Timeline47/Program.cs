using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Timeline47.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.Configure<SeedDataSettings>(builder.Configuration.GetSection("SeedData"));

var env = builder.Environment;
string connectionString;
if (env.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DevConnection")
        ?? throw new Exception("DevConnection is not set in appsettings.json");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception("DefaultConnection is not set in appsettings.json");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

await Seeder.SeedAsync(app.Services);

app.UseFastEndpoints(c =>
{
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
app.Run();