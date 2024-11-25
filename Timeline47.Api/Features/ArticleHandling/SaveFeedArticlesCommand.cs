using System.ServiceModel.Syndication;
using FastEndpoints;
using FluentResults;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.ArticleHandling;

public class SaveFeedArticlesCommand : ICommand<Result<List<Article>>>
{
    public NewsSource NewsSource { get; set; }
    public SyndicationFeed Feed { get; set; }
}