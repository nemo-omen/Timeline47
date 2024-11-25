using Timeline47.Api.Models;

namespace Timeline47.Api.Features.ArticleHandling.GetArticle;

public record GetArticleResponse
{
    public required string Title { get; init; }
    public string? Summary { get; init; }
    public required string Url { get; init; }
    public DateTime PublicationDate { get; init; }
    public required NewsSource NewsSource { get; init; }
    public ICollection<Subject> Subjects { get; init; } = [];
}