using System.ServiceModel.Syndication;
using FluentResults;
using Timeline47.Api.Models;
using Timeline47.Shared.Infrastructure;

namespace Timeline47.Api.Features.NewsGathering;

public interface IFeedService
{
    Task<List<Result<SyndicationFeed>>> GetFeedsFromNewsSourcesAsync(List<NewsSource> newsSources);
    Task<Result<SyndicationFeed>> GetNewsSourceFeedAsync(NewsSource newsSource);
}

public class FeedService : IFeedService
{
    private readonly IFeedReader _feedReader;
    
    public FeedService()
    {
        _feedReader = new FeedReader(
            new HttpClient(),
            RetryPipelineFactory.CreateExponentialBackoffPipeline());
    }

    /// <summary>
    /// Retrieves the syndication feed results from a list of news sources asynchronously.
    /// </summary>
    /// <param name="newsSources">The list of news sources to retrieve feeds from.</param>
    /// <returns>A list of results containing the syndication feed or an error message.</returns>
    public async Task<List<Result<SyndicationFeed>>> GetFeedsFromNewsSourcesAsync(List<NewsSource> newsSources)
    {
        var feedResults = new List<Result<SyndicationFeed>>();
        const int batchSize = 10;
        foreach (var batch in newsSources.Chunk(batchSize))
        {
            var tasks = batch.Select(async newsSource => await GetNewsSourceFeedAsync(newsSource));
            feedResults.AddRange(await Task.WhenAll(tasks));
        }
        return feedResults;
    }
    
    /// <summary>
    /// Retrieves the syndication feed result for a given news source asynchronously.
    /// </summary>
    /// <param name="newsSource">The news source to retrieve the feed from.</param>
    /// <returns>A result containing the syndication feed or an error message.</returns>
    public async Task<Result<SyndicationFeed>> GetNewsSourceFeedAsync(NewsSource newsSource)
    {
        var feedResult = await _feedReader.ReadAsync(newsSource.Url);
        return feedResult;
    }
}