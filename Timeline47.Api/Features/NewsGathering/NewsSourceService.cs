using FluentResults;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.NewsGathering;

public interface INewsSourceService
{
    Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid id);
    Task<Result<NewsSource>> GetNewsSourceByUrlAsync(string url);
    Task<List<NewsSource>> GetNewsSourcesAsync();
}

public class NewsSourceService : INewsSourceService
{
    private readonly INewsSourceRepository _newsSourceRepository;
    
    public NewsSourceService(INewsSourceRepository newsSourceRepository)
    {
        _newsSourceRepository = newsSourceRepository;
    }
    
    public async Task<List<NewsSource>> GetNewsSourcesAsync()
    {
        var newsSourceResult = await _newsSourceRepository.GetNewsSourcesAsync();
        return !newsSourceResult.IsSuccess ? [] : newsSourceResult.Value;
    }
    
    public async Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid id)
    {
        
        return await _newsSourceRepository.GetNewsSourceByIdAsync(id);
    }
    
    public async Task<Result<NewsSource>> GetNewsSourceByUrlAsync(string url)
    {
        return await _newsSourceRepository.GetNewsSourceByUrlAsync(url);
    }
}