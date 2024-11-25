using System.ServiceModel.Syndication;
using FastEndpoints;
using FluentResults;
using Timeline47.Api.Features.ArticleHandling;
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


        var saveArticlesCommands = new List<SaveFeedArticlesCommand>();
        foreach (var feed in feeds)
        {
            var newsSource = newsSources.FirstOrDefault(ns => new Uri(ns.Url).Authority == feed.Links[0].Uri.Authority);
            
            if (newsSource is not null)
            {
                saveArticlesCommands.Add(new SaveFeedArticlesCommand
                {
                    NewsSource = newsSource,
                    Feed = feed
                });
            }
        }
        
        foreach(var cmd in saveArticlesCommands)
        {
            var articlesResult = await cmd.ExecuteAsync(cancellationToken);
            if (articlesResult.IsSuccess)
            {
                Banner.Log($"Saved {articlesResult.Value.Count} articles from {cmd.NewsSource.Name}.");
            }
        }
        // process feeds
        // what does this mean?
        // 1. Each feed needs to be broken down into a collection of Articles -- easy Select or foreach
        // 2. Each Article needs to be stored in the database -- easy Insert or Add
        //    - if we're planning to store a summary of the article, we may need to process the article text
        //      using a separate SLM/LLM model first
        // 3. Each article needs to be processed with NER -- use bert within a service (NerService?)
        //    - need to decide what we're returning from the NerService method
        //    - list of entities and their position in the text?
        return Result.Ok("News gathering initialized." + " " + feeds.Count + " feeds gathered.");
    }
}