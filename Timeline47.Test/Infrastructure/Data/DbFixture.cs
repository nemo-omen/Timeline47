using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timeline47.Api.Data;
using Timeline47.Shared.SeedData;
using Xunit;

namespace Timeline47.Test.Infrastructure.Data;

public class DbFixture : IAsyncLifetime
{
    public ApplicationDbContext DbContext { get; private set; }
    private readonly IContainer _container;
    public string ConnectionString { get; private set; }


    public DbFixture()
    {
        _container = new ContainerBuilder()
            .WithImage("postgres:latest")
            .WithPortBinding(5432, true) // Bind random port to 5432
            .WithEnvironment("POSTGRES_USER", "Tl47TestAdmin")
            .WithEnvironment("POSTGRES_PASSWORD", "YourPassword123!")
            .WithEnvironment("POSTGRES_DB", "TestDatabase")
            // .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(5432)))
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public async Task InitializeAsync()
    {
        // Set the environment to "Test"
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        // Start the container
        await _container.StartAsync();

        // Get the dynamically mapped port
        var host = _container.Hostname;
        var port = _container.GetMappedPublicPort(5432);
        var uniqueDbName = $"TestDatabase_{Guid.NewGuid()}";
        // Set the connection string
        ConnectionString =
            $"Host={host};Port={port};Database={uniqueDbName};Username=Tl47TestAdmin;Password=YourPassword123!;";
        // Create DbContext with container's connection string
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        DbContext = new ApplicationDbContext(options);

        Console.WriteLine($"Starting container on {ConnectionString}");

        // Ensure database schema
        await DbContext.Database.EnsureCreatedAsync();

        // Seed test data
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        // Seed test data
        
        // Create a service provider for dependency injection
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<DbContextOptions<ApplicationDbContext>>(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options);
        serviceCollection.AddDbContext<ApplicationDbContext>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Call the Seeder
        await Seeder.SeedAsync(serviceProvider);
        
        // await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        // Drop the database to clean up data
        await DbContext.Database.EnsureDeletedAsync();

        // Cleanup container
        await _container.StopAsync();
        await _container.DisposeAsync();

        // Dispose DbContext
        await DbContext.DisposeAsync();
    }
}