using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Reviews.Infrastructure.Reviews;

internal sealed class VerifiedPurchaseRepository(ReviewsDbContext context) : IVerifiedPurchaseRepository
{
    public async Task<bool> ExistsAsync(
        Guid gameId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await context.VerifiedPurchases
            .AnyAsync(v => v.GameId == gameId && v.UserId == userId, cancellationToken);
    }

    public void Insert(VerifiedPurchase purchase)
    {
        context.VerifiedPurchases.Add(purchase);
    }
}
