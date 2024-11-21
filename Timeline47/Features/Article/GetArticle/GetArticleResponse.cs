using Timeline47.Models;

namespace Timeline47.Features.Article.GetArticle;

public record GetArticleResponse
{
    public string Title { get; init; }
    public string Summary { get; init; }
    public string Url { get; init; }
    public DateTime PublicationDate { get; init; }
    public NewsSource NewsSource { get; init; }
    public ICollection<Subject> Subjects { get; init; }
}