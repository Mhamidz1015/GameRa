using Bogus;
using GameRa.Modules.Store.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Store.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly StoreDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    }

    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM Store.inbox_message_consumers;
            DELETE FROM Store.inbox_messages;
            DELETE FROM Store.outbox_message_consumers;
            DELETE FROM Store.outbox_messages;
            DELETE FROM Store.games;
            DELETE FROM Store.customers;
            DELETE FROM Store.orders;
            DELETE FROM Store.order_items;
            DELETE FROM Store.payments;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
