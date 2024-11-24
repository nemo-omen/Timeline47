using System.ServiceModel.Syndication;
using FastEndpoints;
using Timeline47.Api.Models;

namespace Timeline47.Api.Features.NewsGathering.GetAllFeeds;

public class GetAllFeedsCommand : ICommand<List<SyndicationFeed>>
{
    public List<NewsSource> NewsSources { get; set; } = [];
}