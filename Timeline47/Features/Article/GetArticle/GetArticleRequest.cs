namespace Timeline47.Features.Article.GetArticle;

public record GetArticleRequest
{
    public required string ArticleId { get; init; }
}