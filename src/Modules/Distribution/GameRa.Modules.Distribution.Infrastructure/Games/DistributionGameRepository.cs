using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Distribution.Infrastructure.Games;

internal sealed class DistributionGameRepository(DistributionDbContext context) : IDistributionGameRepository
{
    public async Task<bool> ExistsAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.Games.AnyAsync(g => g.GameId == gameId, cancellationToken);
    }

    public void Insert(DistributionGame game)
    {
        context.Games.Add(game);
    }
}
