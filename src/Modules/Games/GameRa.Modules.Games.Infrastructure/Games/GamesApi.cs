using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.Infrastructure.Database;
using GameRa.Modules.Games.PublicApi;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Games.Infrastructure.Games;

internal sealed class GamesApi(GamesDbContext context) : IGamesApi
{
    public async Task<GameApiResponse?> GetGameApiAsync(
        Guid gameId,
        CancellationToken cancellationToken = default)
    {
        Game? game = await context.Games
            .SingleOrDefaultAsync(g => g.Id == gameId, cancellationToken);

        if (game is null)
            return null;

        return new GameApiResponse(game.Id, game.CategoryId, game.Title, game.BasePrice);
    }

    public async Task<bool> GameExistsAsync(
        Guid gameId,
        CancellationToken cancellationToken = default)
    {
        return await context.Games
            .AnyAsync(g => g.Id == gameId, cancellationToken);
    }
}
