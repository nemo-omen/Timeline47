using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Timeline47.Api.Features.NewsGathering;
using Timeline47.Shared.Infrastructure;
using Xunit;

namespace Timeline47.Test.Features.NewsGathering;

public class FeedReaderTests
{
    [Fact]
    public void ReadAsync_NullUrl_ThrowsArgumentException()
    {
        // Arrange
        var feedReader = new FeedReader(new HttpClient(), RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var act = () => feedReader.ReadAsync(null).Wait();
        #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ReadAsync_EmptyUrl_ThrowsArgumentException()
    {
        // Arrange
        var feedReader = new FeedReader(new HttpClient(), RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var act = () => feedReader.ReadAsync(string.Empty).Wait();
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ReadAsync_InvalidUrl_ThrowsArgumentException()
    {
        // Arrange
        var feedReader = new FeedReader(new HttpClient(), RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var act = () => feedReader.ReadAsync("invalid-url").Wait();
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public async Task ReadAsync_ValidFeed_ReturnsSuccessResult()
    {
        // Arrange
        const string validFeedXml = @"<?xml version='1.0' encoding='utf-8'?>
            <rss version='2.0'>
                <channel>
                    <title>Feed Title</title>
                    <link>https://example.com</link>
                    <description>Feed Description</description>
                    <item>
                        <title>Item Title</title>
                        <link>https://example.com/item</link>
                        <description>Item Description</description>
                        <pubDate>Mon, 01 Jan 2000 00:00:00 GMT</pubDate>
                    </item>
                </channel>
            </rss>";

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(validFeedXml),
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Text.Should().Be("Feed Title");
    }
    
    [Fact]
    public async Task ReadAsync_InValidFeed_ReturnsFailResult()
    {
        // Arrange
        const string validFeedXml = @"<?xml version='1.0' encoding='utf-8'?>
            <rss version='2.0'>
                <channel>
                    <title>Feed Title</title>
                    <link>https://example.com</link>
                    <description>Feed Description</description>
                    <item>
                        <title>Item Title</title>
                        <link>https://example.com/item</link>
                        <description>Item Description</description>
                        <pubDate>Mon, 01 Jan 2000 00:00:00 GMT</pubDate>
                    </item>
                </channel>
            </rss";

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(validFeedXml),
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_FailedRequest_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = "Internal Server Error",
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_CanceledRequest_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_TimeoutRequest_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_EmptyStringResponse_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty),
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_NullResponse_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpResponseMessage)null);
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_NullContent_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = null,
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_NullStream_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            });
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_AnyException_ReturnsFailResult()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception());
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var feedReader = new FeedReader(httpClient, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_NullRetryPipeline_ReturnsFailResult()
    {
        // Arrange
        var httpClient = new HttpClient();
        var feedReader = new FeedReader(httpClient, null);
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task ReadAsync_NullHttpClient_ReturnsFailResult()
    {
        // Arrange
        var feedReader = new FeedReader(null, RetryPipelineFactory.CreateLowRetryPipeline());
        
        // Act
        var result = await feedReader.ReadAsync("https://example.com");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }
}