using System.ServiceModel.Syndication;
using FluentResults;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.ArticleHandling;

public interface IArticleService
{
    Task<Result<List<Article>>> ConvertSyndicationFeedToArticlesAsync(SyndicationFeed feed, NewsSource newsSource);
}

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    
    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<Result<List<Article>>> ConvertSyndicationFeedToArticlesAsync(SyndicationFeed feed, NewsSource newsSource)
    {
        var items = feed.Items;
        var articles = new List<Article>();

        foreach (var item in items)
        {
            var article = new Article
            {
                Id = Guid.Empty,
                Title = item.Title.Text,
                Summary = "",
                NewsSourceId = newsSource.Id,
                NewsSource = newsSource,
                PublicationDate = item.PublishDate.DateTime,
                Url = item.Links.First().Uri.ToString(),
            };
            articles.Add(article);
        }

        return Result.Ok(articles);
    }
}