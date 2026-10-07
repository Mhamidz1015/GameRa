using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace GameRa.Modules.Distribution.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:latest")
        .WithDatabase("gamera")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ConnectionStrings:Cache", _redisContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("Distribution:Outbox:IntervalInSeconds", "1");
        Environment.SetEnvironmentVariable("Distribution:Inbox:IntervalInSeconds", "1");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFileStorage>();
            services.AddSingleton<FakeFileStorage>();
            services.AddSingleton<IFileStorage>(sp => sp.GetRequiredService<FakeFileStorage>());
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        _ = CreateClient();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
