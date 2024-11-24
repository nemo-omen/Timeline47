using System.ServiceModel.Syndication;
using FastEndpoints;
using Timeline47.Api.Shared.Util;

namespace Timeline47.Api.Features.NewsGathering.GetAllFeeds;

public class GetAllFeedsCommandHandler : ICommandHandler<GetAllFeedsCommand, List<SyndicationFeed>>
{
    private readonly IFeedService _feedService;
    
    public GetAllFeedsCommandHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _feedService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IFeedService>();
    }
    
    public async Task<List<SyndicationFeed>> ExecuteAsync(GetAllFeedsCommand command, CancellationToken cancellationToken)
    {
        // Get feeds from news sources
        Banner.Log("Getting feeds from news sources...");
        var feedResults = await _feedService.GetFeedsFromNewsSourcesAsync(command.NewsSources);
        
        // Separate successful feeds from failed feeds
        var failedFeeds = feedResults
            .Where(feedResult => !feedResult.IsSuccess)
            .ToList();
        
        var successfulFeeds = feedResults
            .Where(feedResult => feedResult.IsSuccess)
            .Select(feedResult => feedResult.Value)
            .ToList();
        
        // Todo: Log and register feed failures
        Banner.Log($"Feed results:\nFailed feeds: {failedFeeds.Count}\nSuccessful Feeds: {successfulFeeds.Count}");
        
        // return successful feeds for processing
        return successfulFeeds;
    }
}