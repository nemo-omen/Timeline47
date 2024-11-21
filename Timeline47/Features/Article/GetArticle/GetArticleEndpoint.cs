using FastEndpoints;
using Timeline47.Models;

namespace Timeline47.Features.Article.GetArticle;

public class GetArticleEndpoint : EndpointWithoutRequest<GetArticleResponse>
{
    public override void Configure() => Get("/article/{articleId}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        await SendAsync(new GetArticleResponse
        {
            Title = "Article Title",
            Summary = "Article Summary",
            Url = "https://example.com",
            PublicationDate = DateTime.UtcNow,
            NewsSource = new NewsSource
            {
                Name = "News Source",
                Url = "https://example.com"
            },
            Subjects = new List<Subject>
            {
                new Subject
                {
                    Name = "Subject 1",
                    Type = new SubjectType
                    {
                        Id = Guid.NewGuid(),
                        Type = "Person",
                    }
                },
                new Subject
                {
                    Name = "Subject 2",
                    Type = new SubjectType
                    {
                        Id = Guid.NewGuid(),
                        Type = "Organization",
                    }
                }
            }
        });
    }
}