using Timeline47.Api.Features.NewsGathering;
using Timeline47.Api.Models;
using Timeline47.Test.Infrastructure.Data;
using Xunit;

namespace Timeline47.Test.Features.NewsGathering;

public class NewsSourceRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    
    public NewsSourceRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task GetNewsSourcesAsync_Should_ReturnSuccessResultWithNewsSources()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        
        // Assert
        Assert.NotNull(newsSourcesResult);
        Assert.True(newsSourcesResult.IsSuccess);
        Assert.NotEmpty(newsSourcesResult.Value);
    }
    
    [Fact]
    public async Task GetNewsSourceByIdAsyncWithBadId_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByIdAsync(Guid.NewGuid());
        
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
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByIdAsync(newsSourcesResult.Value.First().Id);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
        Assert.Equal(newsSourcesResult.Value.First().Id, newsSourceResult.Value.Id);
    }
    
    [Fact]
    public async Task GetNewsSourceByUrlAsyncWithBadUrl_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByUrlAsync("https://badurl.com");
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task GetNewsSourceByUrlAsyncWithGoodUrl_Should_ReturnSuccessResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByUrlAsync(newsSourcesResult.Value.First().Url);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
        Assert.Equal(newsSourcesResult.Value.First().Url, newsSourceResult.Value.Url);
    }
    
    [Fact]
    public async Task AddNewsSourceAsyncWithExistingUrl_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSource = newsSourcesResult.Value.First();
        var newsSourceResult = await newsSourceRepository.AddNewsSourceAsync(newsSource);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source already exists", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task AddNewsSourceAsyncWithNewUrl_Should_ReturnSuccessResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSource = new NewsSource
        {
            Id = Guid.NewGuid(),
            Url = "https://newurl.com",
            Name = "New Source"
        };
        var newsSourceResult = await newsSourceRepository.AddNewsSourceAsync(newsSource);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
        Assert.Equal(newsSource.Url, newsSourceResult.Value.Url);
    }
    
    [Fact]
    public async Task UpdateNewsSourceAsyncWithBadId_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSource = new NewsSource
        {
            Id = Guid.NewGuid(),
            Url = "https://newurl.com",
            Name = "New Source"
        };
        var newsSourceResult = await newsSourceRepository.UpdateNewsSourceAsync(newsSource);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task UpdateNewsSourceAsyncWithGoodId_Should_ReturnSuccessResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSource = newsSourcesResult.Value.First();
        newsSource.Name = "Updated Source";
        var newsSourceResult = await newsSourceRepository.UpdateNewsSourceAsync(newsSource);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
        Assert.Equal(newsSource.Name, newsSourceResult.Value.Name);
    }
    
    [Fact]
    public async Task DeleteNewsSourceAsyncWithBadId_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourceResult = await newsSourceRepository.DeleteNewsSourceAsync(Guid.NewGuid());
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task DeleteNewsSourceAsyncWithGoodId_Should_ReturnSuccessResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSourceResult = await newsSourceRepository.DeleteNewsSourceAsync(newsSourcesResult.Value.First().Id);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.True(newsSourceResult.IsSuccess);
    }
    
    [Fact]
    public async Task DeleteNewsSourceAsyncWithGoodId_Should_DeleteNewsSource()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourcesResult = await newsSourceRepository.GetNewsSourcesAsync();
        var newsSourceId = newsSourcesResult.Value.First().Id;
        await newsSourceRepository.DeleteNewsSourceAsync(newsSourceId);
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByIdAsync(newsSourceId);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
    
    [Fact]
    public async Task GetNewsSourceByIdAsyncWithEmptyId_Should_ReturnFailedResult()
    {
        // Arrange
        var context = _fixture.DbContext;
        var newsSourceRepository = new NewsSourceRepository(context);
        
        // Act
        var newsSourceResult = await newsSourceRepository.GetNewsSourceByIdAsync(Guid.Empty);
        
        // Assert
        Assert.NotNull(newsSourceResult);
        Assert.False(newsSourceResult.IsSuccess);
        Assert.Equal("News source not found", newsSourceResult.Errors.First().Message);
    }
}