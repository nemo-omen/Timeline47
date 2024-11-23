namespace Timeline47.Api.Features.Article.GetArticle;

public record GetArticleRequest
{
    public required string ArticleId { get; init; }
}