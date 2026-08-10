using Bogus;
using GameRa.Modules.Discounts.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Discounts.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly DiscountDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
    }

    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM discount.inbox_message_consumers;
            DELETE FROM discount.inbox_messages;
            DELETE FROM discount.outbox_message_consumers;
            DELETE FROM discount.outbox_messages;
            DELETE FROM discount.discount;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
