using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Timeline47.Api.Data;
using Timeline47.Api.Features.ArticleHandling;
using Timeline47.Api.Features.NewsGathering;
using Timeline47.Api.Models;
using Timeline47.Api.Shared;
using Timeline47.Api.Shared.Services;
using Timeline47.Shared.SeedData;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddFastEndpoints()
    .AddJobQueues<JobRecord, JobStorageProvider>();

builder.Services.Configure<SeedDataSettings>(builder.Configuration.GetSection("SeedData"));

// local services and repositories
builder.Services.AddScoped<INewsSourceRepository, NewsSourceRepository>();
builder.Services.AddScoped<INewsSourceService, NewsSourceService>();
builder.Services.AddScoped<IFeedService, FeedService>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();

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

builder.Services.AddHostedService<PipelineInitializerHostedService>();

var app = builder.Build();

await Seeder.SeedAsync(app.Services);

app.UseDefaultExceptionHandler()
    .UseFastEndpoints(c =>
    {
        c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .UseJobQueues();

app.Run();