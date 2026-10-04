using Bogus;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Reviews.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly ReviewsDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<ReviewsDbContext>();
    }

    protected async Task SeedVerifiedPurchaseAsync(Guid gameId, Guid userId)
    {
        var verifiedPurchase = VerifiedPurchase.Create(gameId, userId, DateTime.UtcNow);
        await DbContext.Set<VerifiedPurchase>().AddAsync(verifiedPurchase);
        await DbContext.SaveChangesAsync();
    }

    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM reviews.inbox_message_consumers;
            DELETE FROM reviews.inbox_messages;
            DELETE FROM reviews.outbox_message_consumers;
            DELETE FROM reviews.outbox_messages;
            DELETE FROM reviews.reviews;
            DELETE FROM reviews.verified_purchases;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}