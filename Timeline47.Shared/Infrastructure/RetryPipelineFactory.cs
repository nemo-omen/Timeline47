using Polly;
using Polly.Retry;

namespace Timeline47.Shared.Infrastructure;

public static class RetryPipelineFactory
{
    public static ResiliencePipeline<HttpResponseMessage> CreateLowRetryPipeline()
    {
        return new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(static result => !result.IsSuccessStatusCode),
                MaxRetryAttempts = 1,
                Delay = TimeSpan.FromSeconds(0),
            })
            .Build();
    }
    
    public static ResiliencePipeline<HttpResponseMessage> CreateDefaultPipeline()
    {
        return new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
            })
            .Build();
    }

    public static ResiliencePipeline<HttpResponseMessage> CreateExponentialBackoffPipeline()
    {
        return new ResiliencePipelineBuilder<HttpResponseMessage>()
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
}