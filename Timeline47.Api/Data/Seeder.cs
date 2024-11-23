using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Timeline47.Api.Models;

namespace Timeline47.Api.Data;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var seedSettings = scope.ServiceProvider.GetRequiredService<IOptions<SeedDataSettings>>().Value;
        var newsSourcesSeed = LoadSeedData<NewsSource>(seedSettings.NewsSourcesSeed); 
        var dataSourcesSeed = LoadSeedData<DataSource>(seedSettings.DataSourcesSeed);
        var existingNewsSourceRecords = await context.NewsSources.ToListAsync();
        var existingDataSourceRecords = await context.DataSources.ToListAsync();

        foreach (var nsSeed in newsSourcesSeed)
        {
            var existingNewsSource = existingNewsSourceRecords.FirstOrDefault(ns => ns.Url == nsSeed.Url);
            if (existingNewsSource is null)
            {
                var newsSourceWithId = new NewsSource
                {
                    Id = Guid.NewGuid(),
                    Name = nsSeed.Name,
                    Url = nsSeed.Url,
                    FeedUrl = nsSeed.FeedUrl
                };

                await context.AddAsync(newsSourceWithId);
            }
            else
            {
                existingNewsSource.Name = nsSeed.Name;
                existingNewsSource.Url = nsSeed.Url;
                existingNewsSource.FeedUrl = nsSeed.FeedUrl;
            }
        }
        
        
        foreach (var dsSeed in dataSourcesSeed)
        {
            var existingDataSource = existingDataSourceRecords.FirstOrDefault(ns => ns.Url == dsSeed.Url);
            if (existingDataSource is null)
            {
                var dataSourceWithId = new DataSource {
                    Id = Guid.NewGuid(),
                    Name = dsSeed.Name,
                    ShortName = dsSeed.ShortName,
                    Url = dsSeed.Url,
                    DataUrl = dsSeed.DataUrl,
                    Category = dsSeed.Category
                };

                await context.AddAsync(dataSourceWithId);
            }
            else
            {
                existingDataSource.Name = dsSeed.Name;
                existingDataSource.ShortName = dsSeed.ShortName;
                existingDataSource.Url = dsSeed.Url;
                existingDataSource.DataUrl = dsSeed.DataUrl;
                existingDataSource.Category = dsSeed.Category;
            }
        }
        
        await context.SaveChangesAsync();
    }

    private static bool AreNewsSourcesEqual(NewsSource ns1, NewsSource ns2)
    {
        return ns1.Name == ns2.Name && ns1.Url == ns2.Url;
    }
    
    private static bool AreDataSourcesEqual(DataSource ds1, DataSource ds2)
    {
        return ds1.Name == ds2.Name && ds1.Url == ds2.Url;
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