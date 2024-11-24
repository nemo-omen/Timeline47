using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Timeline47.Test.Infrastructure.Data;

[Collection("Integration Tests")]
public class DbFixtureTests
{
    private readonly DbFixture _fixture;

    public DbFixtureTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DbFixture_Should_InitializeDatabaseSeedData()
    {
        // Arrange
        var context = _fixture.DbContext;

        // Act
        var newsSources = await context.NewsSources.ToListAsync();
        var dataSources = await context.DataSources.ToListAsync();

        // Assert
        Assert.NotNull(newsSources);
        Assert.NotNull(dataSources);
        Assert.NotEmpty(newsSources);
        Assert.NotEmpty(dataSources);
        Assert.Equal(6, dataSources.Count);
        Assert.Equal(13, newsSources.Count);
    }
}