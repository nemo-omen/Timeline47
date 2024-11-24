using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Timeline47.Api.Models;
using Timeline47.Shared;
using Timeline47.Shared.SeedData;

namespace Timeline47.Api.Data;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var newsSourceSeedDataPath = SeedDataHelper.GetSeedDataPath("NewsSources.json");
        var dataSourceSeedDataPath = SeedDataHelper.GetSeedDataPath("DataSources.json");
        var seedSettings = new SeedDataSettings
        {
            NewsSourcesSeed = newsSourceSeedDataPath,
            DataSourcesSeed = dataSourceSeedDataPath
        };
        
        var newsSourcesSeed = SeedDataHelper.LoadSeedData<NewsSource>(seedSettings.NewsSourcesSeed); 
        var dataSourcesSeed = SeedDataHelper.LoadSeedData<DataSource>(seedSettings.DataSourcesSeed);
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
}