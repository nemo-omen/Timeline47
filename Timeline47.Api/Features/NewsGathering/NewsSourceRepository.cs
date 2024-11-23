using FluentResults;
using Microsoft.EntityFrameworkCore;
using Timeline47.Api.Data;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.NewsGathering;

public interface INewsSourceRepository
{
    Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid newsSourceId);
    Task<Result<NewsSource>> GetNewsSourceByUrlAsync(string url);
    Task<Result<NewsSource>> AddNewsSourceAsync(NewsSource newsSource);
    Task<Result<NewsSource>> UpdateNewsSourceAsync(NewsSource newsSource);
    Task<Result> DeleteNewsSourceAsync(Guid newsSourceId);
    Task<Result<List<NewsSource>>> GetNewsSourcesAsync();
}

public class NewsSourceRepository
{
    private readonly ApplicationDbContext _context;
    
    public NewsSourceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid newsSourceId)
    {
        var source = await _context.NewsSources
            .FirstOrDefaultAsync(ns => ns.Id == newsSourceId);
        
        if (source is null) return Result.Fail<NewsSource>("News source not found");
        
        return Result.Ok(source);
    }
    
    public async Task<Result<NewsSource>> GetNewsSourceByUrlAsync(string url)
    {
        var source = await _context.NewsSources
            .FirstOrDefaultAsync(ns => ns.Url == url);
        
        if (source is null) return Result.Fail<NewsSource>("News source not found");
        
        return Result.Ok(source);
    }
    
    public async Task<Result<NewsSource>> AddNewsSourceAsync(NewsSource newsSource)
    {
        var existingSource = await _context.NewsSources
            .FirstOrDefaultAsync(ns => ns.Url == newsSource.Url);
        
        if (existingSource is not null) return Result.Fail<NewsSource>("News source already exists");
        
        newsSource.Id = Guid.NewGuid();
        await _context.AddAsync(newsSource);
        await _context.SaveChangesAsync();
        
        return Result.Ok(newsSource);
    }
    
    public async Task<Result<NewsSource>> UpdateNewsSourceAsync(NewsSource newsSource)
    {
        var existingSource = await _context.NewsSources
            .FirstOrDefaultAsync(ns => ns.Id == newsSource.Id);
        
        if (existingSource is null) return Result.Fail<NewsSource>("News source not found");
        
        existingSource.Name = newsSource.Name;
        existingSource.Url = newsSource.Url;
        existingSource.FeedUrl = newsSource.FeedUrl;
        
        await _context.SaveChangesAsync();
        
        return Result.Ok(existingSource);
    }
    
    public async Task<Result> DeleteNewsSourceAsync(Guid newsSourceId)
    {
        var existingSource = await _context.NewsSources
            .FirstOrDefaultAsync(ns => ns.Id == newsSourceId);
        
        if (existingSource is null) return Result.Fail("News source not found");
        
        _context.Remove(existingSource);
        await _context.SaveChangesAsync();
        
        return Result.Ok();
    }
    
    public async Task<Result<List<NewsSource>>> GetNewsSourcesAsync()
    {
        var sources = await _context.NewsSources.ToListAsync();
        
        return Result.Ok(sources);
    }
}