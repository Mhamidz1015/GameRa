using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Distribution.Infrastructure.GameVersions;

internal sealed class GameVersionRepository(DistributionDbContext context) : IGameVersionRepository
{
    public async Task<GameVersion?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.GameVersions
            .Include(v => v.Files)
            .SingleOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid gameId, VersionNumber number, CancellationToken cancellationToken = default)
    {
        return await context.GameVersions.AnyAsync(
            v => v.GameId == gameId && v.Major == number.Major && v.Minor == number.Minor && v.Patch == number.Patch,
            cancellationToken);
    }

    public void Insert(GameVersion gameVersion)
    {
        context.GameVersions.Add(gameVersion);
    }
}
