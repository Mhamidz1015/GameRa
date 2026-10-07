using Bogus;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;
using GameRa.Modules.Distribution.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Distribution.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly DistributionDbContext DbContext;
    protected readonly FakeFileStorage FileStorage;
    protected readonly IGameOwnershipReader OwnershipReader;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<DistributionDbContext>();
        FileStorage = _scope.ServiceProvider.GetRequiredService<FakeFileStorage>();
        OwnershipReader = _scope.ServiceProvider.GetRequiredService<IGameOwnershipReader>();
    }

    protected static string Sha256() => Faker.Random.Hexadecimal(64, string.Empty).ToLowerInvariant();

    protected async Task<int> CountOutboxMessagesAsync(string type, Guid versionId)
    {
        string pattern = $"%{versionId}%";

        return await DbContext.Database
            .SqlQuery<int>($"""
                SELECT count(*)::int AS "Value"
                FROM distribution.outbox_messages
                WHERE type = {type} AND content::text LIKE {pattern}
                """)
            .SingleAsync();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
