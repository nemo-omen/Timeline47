namespace Timeline47.Api.Features.ArticleHandling.GetArticle;

public record GetArticleRequest
{
    public required string ArticleId { get; init; }
}