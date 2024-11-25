using FastEndpoints;
using FluentResults;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.ArticleHandling;

public class SaveFeedArticlesCommandHandler : ICommandHandler<SaveFeedArticlesCommand, Result<List<Article>>>
{
    private readonly IArticleService _articleService;
    
    public SaveFeedArticlesCommandHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _articleService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IArticleService>();
    }

    public async Task<Result<List<Article>>> ExecuteAsync(SaveFeedArticlesCommand command, CancellationToken ct)
    {
        var articlesResult = await _articleService
            .ConvertSyndicationFeedToArticlesAsync(command.Feed, command.NewsSource);
        return articlesResult;
        // Save the feed articles
    }
}