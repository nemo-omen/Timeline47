using System.ServiceModel.Syndication;
using System.Xml;
using FluentResults;
using Polly;
using Polly.Retry;

namespace Timeline47.Api.Features.NewsGathering;

public interface IFeedReader
{
    Task<Result<SyndicationFeed>> ReadAsync(string url, CancellationToken cancellationToken = default);
}


public class FeedReader : IFeedReader
{
    private readonly ResiliencePipeline<HttpResponseMessage> _retryPipeline;
    private readonly HttpClient _httpClient;
    public FeedReader(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(static result => !result.IsSuccessStatusCode),
                Delay = TimeSpan.FromSeconds(3),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
            })
            .Build();
    }
    
    /// <summary>
    /// Reads a syndication feed from the specified URL asynchronously. This method uses a retry policy
    /// to handle transient errors and will retry up to 3 times with an exponential backoff delay.
    /// </summary>
    /// <param name="url">The URL of the syndication feed to read.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the syndication feed or an error message.</returns>
    public async Task<Result<SyndicationFeed>> ReadAsync(string url, CancellationToken cancellationToken = default)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));
        }
        
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("The provided URL is not valid. Only HTTP and HTTPS are supported.", nameof(url));
        }

        // Fetch the feed
        try
        {
            // using var httpClient = new HttpClient();
            var pipelineResponse = await _retryPipeline.ExecuteAsync(async token =>
            {
                var response = await _httpClient.GetAsync(url, token);
                return response;
            }, cancellationToken);
            
            if (!pipelineResponse.IsSuccessStatusCode)
            {
                return Result.Fail<SyndicationFeed>($"Failed to fetch the feed for {url}.")
                    .WithError(new ExceptionalError("HttpResponseException", new Exception(pipelineResponse.ReasonPhrase)));
            }

            await using var stream = await pipelineResponse.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = XmlReader.Create(stream);

            var feed = SyndicationFeed.Load(reader);
            return Result.Ok(feed);
        }
        // Handle exceptions
        catch (HttpRequestException e)
        {
            return Result.Fail<SyndicationFeed>($"Failed to fetch the feed for {url}")
                .WithError(new ExceptionalError("HttpRequestException", e));
        }
        catch (XmlException e)
        {
            return Result.Fail<SyndicationFeed>($"Failed to parse the feed for {url}")
                .WithError(new ExceptionalError("XmlException", e));
        }
        catch (Exception e)
        {
            return Result.Fail<SyndicationFeed>("An unexpected error occurred while fetching the feed")
                .WithError(new ExceptionalError("Exception", e));
        }
    }
}