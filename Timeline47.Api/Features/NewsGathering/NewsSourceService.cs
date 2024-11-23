using FluentResults;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.NewsGathering;

public interface INewsSourceService
{
    Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid id);
}

public class NewsSourceService
{
    private readonly INewsSourceRepository _newsSourceRepository;
    
    public NewsSourceService(INewsSourceRepository newsSourceRepository)
    {
        _newsSourceRepository = newsSourceRepository;
    }
    
    public async Task<Result<NewsSource>> GetNewsSourceByIdAsync(Guid id)
    {
        return await _newsSourceRepository.GetNewsSourceByIdAsync(id);
    }
}