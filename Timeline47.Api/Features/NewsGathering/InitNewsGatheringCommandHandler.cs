using System.ServiceModel.Syndication;
using FastEndpoints;
using FluentResults;
using Timeline47.Api.Features.NewsGathering.GetAllFeeds;
using Timeline47.Api.Features.NewsGathering.GetAllNewsSources;
using Timeline47.Api.Shared.Util;

namespace Timeline47.Api.Features.NewsGathering;

public class InitNewsGatheringCommandHandler : ICommandHandler<InitNewsGatheringCommand, Result<string>>
{
    public async Task<Result<string>> ExecuteAsync(InitNewsGatheringCommand command, CancellationToken cancellationToken)
    {
        // Initialize the news-gathering pipeline
        
        Banner.Log("Initializing news-gathering pipeline...");
        
        var newsSources = await new GetAllNewsSourcesCommand()
            .ExecuteAsync(cancellationToken);
        
        var feeds = await new GetAllFeedsCommand
        {
            NewsSources = newsSources
        }.ExecuteAsync(cancellationToken);
        return Result.Ok("News gathering initialized." + " " + feeds.Count + " feeds gathered.");
    }
}