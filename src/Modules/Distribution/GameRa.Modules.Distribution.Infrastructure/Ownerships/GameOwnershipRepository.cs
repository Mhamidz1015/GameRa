using GameRa.Modules.Distribution.Domain.Ownerships;
using GameRa.Modules.Distribution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Distribution.Infrastructure.Ownerships;

internal sealed class GameOwnershipRepository(DistributionDbContext context) : IGameOwnershipRepository
{
    public async Task<GameOwnership?> GetAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.GameOwnerships.SingleOrDefaultAsync(
            o => o.UserId == userId && o.GameId == gameId,
            cancellationToken);
    }

    public void Insert(GameOwnership gameOwnership)
    {
        context.GameOwnerships.Add(gameOwnership);
    }
}
