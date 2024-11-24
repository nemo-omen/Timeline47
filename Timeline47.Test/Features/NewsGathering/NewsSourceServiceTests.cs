using Microsoft.EntityFrameworkCore;
using Timeline47.Api.Features.NewsGathering;
using Timeline47.Test.Infrastructure.Data;
using Xunit;

namespace Timeline47.Test.Features.NewsGathering;

public class NewsSourceServiceTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    
    public NewsSourceServiceTests(DbFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task GetNewsSourcesAsync_Should_ReturnNewsSources()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        
        // Act
        var newsSources = await newsSourceService.GetNewsSourcesAsync();
        
        // Assert
        Assert.NotNull(newsSources);
        Assert.NotEmpty(newsSources);
    }
    
    [Fact]
    public async Task GetNewsSourceByIdAsyncWithBadId_Should_ReturnFailedResponse()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        
        // Act
        var newsSourceResult = await newsSourceService.GetNewsSourceByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task GetNewsSourceByIdAsyncWithGoodId_Should_ReturnSuccessResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        
        // Act
        var newsSources = await newsSourceService.GetNewsSourcesAsync();
        var newsSourceResult = await newsSourceService.GetNewsSourceByIdAsync(newsSources.First().Id);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
        Assert.NotNull(newsSourceResult.Value);
    }
    
    [Fact]
    public async Task GetNewsSourceByUrlAsyncWithBadUrl_Should_ReturnFailedResponse()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        var newsSourceService = new NewsSourceService(newsSourceRepository);
        
        // Act
        var newsSourceResult = await newsSourceService.GetNewsSourceByUrlAsync("http://badurl.com");
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
}