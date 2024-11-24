using FastEndpoints;
using Timeline47.Api.Features.NewsGathering;

namespace Timeline47.Api.Shared.Services;

public class PipelineInitializerHostedService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize the news-gathering pipeline
        await new InitNewsGatheringCommand().QueueJobAsync(
            expireOn: DateTime.UtcNow.AddSeconds(45),
            ct: cancellationToken
            );
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}