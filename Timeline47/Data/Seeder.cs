using System.Text.Json;
using Microsoft.Extensions.Options;
using Timeline47.Models;

namespace Timeline47.Data;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var seedSettings = scope.ServiceProvider.GetRequiredService<IOptions<SeedDataSettings>>().Value;
        var newsSourcesSeed = LoadSeedData<NewsSource>(seedSettings.NewsSourcesSeed); 
        var dataSourcesSeed = LoadSeedData<DataSource>(seedSettings.DataSourcesSeed);
        
        if (!context.NewsSources.Any())
        {
            var newsSourcesWithId = newsSourcesSeed.Select(ns =>
            {
                ns.Id = Guid.NewGuid();
                return ns;
            });
            Console.WriteLine($"Seeding {newsSourcesWithId.Count()} NewsSources");
            await context.NewsSources.AddRangeAsync(newsSourcesWithId);
        }
        
        if (!context.DataSources.Any())
        {
            var dataSourcesWithId = dataSourcesSeed.Select(ds =>
            {
                ds.Id = Guid.NewGuid();
                return ds;
            });
            Console.WriteLine($"Seeding {dataSourcesWithId.Count()} DataSources");
            await context.DataSources.AddRangeAsync(dataSourcesWithId);
        }
        
        await context.SaveChangesAsync();
    }

    private static List<T> LoadSeedData<T>(string filePath)
    {
        string? jsonData = null;
        try
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading news source data: {filePath}", e);
        }
        
        if(jsonData is null)
        {
            throw new Exception($"News source data is empty: {filePath}");
        }
        
        var data = JsonSerializer.Deserialize<List<T>>(jsonData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        if (data is null)
        {
            throw new Exception($"News source data is empty or invalid: {filePath}");
        }

        return data;
    }
}