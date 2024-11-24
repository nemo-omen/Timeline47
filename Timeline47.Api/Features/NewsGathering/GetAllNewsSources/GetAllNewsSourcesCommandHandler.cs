using FastEndpoints;
using Timeline47.Api.Models;
using Timeline47.Api.Shared.Util;

namespace Timeline47.Api.Features.NewsGathering.GetAllNewsSources;

public class GetAllNewsSourcesCommandHandler : ICommandHandler<GetAllNewsSourcesCommand, List<NewsSource>>
{
    private readonly INewsSourceService _newsSourceService;
    
    public GetAllNewsSourcesCommandHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _newsSourceService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<INewsSourceService>();
    }
    
    public async Task<List<NewsSource>> ExecuteAsync(GetAllNewsSourcesCommand command, CancellationToken cancellationToken)
    {
        Banner.Log("Getting news sources...");
        var newsSources = await _newsSourceService.GetNewsSourcesAsync();
        Banner.Log($"Retrieved {newsSources.Count} news sources.");
        return newsSources;
    }
}