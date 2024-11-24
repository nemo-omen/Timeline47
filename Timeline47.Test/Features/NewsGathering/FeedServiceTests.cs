using JetBrains.Annotations;
using Timeline47.Api.Features.NewsGathering;
using Timeline47.Test.Infrastructure.Data;
using Xunit;

namespace Timeline47.Test.Features.NewsGathering;

[TestSubject(typeof(FeedService))]
public class FeedServiceTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    
    public FeedServiceTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void GetNewsSourceFeedAsync_Should_ReturnResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        var feedService = new FeedService();
        var newsSources = newsSourceService.GetNewsSourcesAsync().Result;
        
        // Act
        var feedResult = feedService.GetNewsSourceFeedAsync(newsSources.First()).Result;
        
        // Assert
        Assert.NotNull(feedResult);
    }
    
    [Fact]
    public async Task GetFeedsFromNewsSourcesAsync_Should_ReturnListOfSyndicationFeedResults()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        var feedService = new FeedService();
        var newsSources = await newsSourceService.GetNewsSourcesAsync();
        
        // Act
        var feedResults = await feedService.GetFeedsFromNewsSourcesAsync(newsSources);
        
        // Assert
        Assert.NotNull(feedResults);
        Assert.NotEmpty(feedResults);
    }
}