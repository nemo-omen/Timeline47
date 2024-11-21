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
    connectionString = builder.Configuration.GetConnectionString("DevConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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