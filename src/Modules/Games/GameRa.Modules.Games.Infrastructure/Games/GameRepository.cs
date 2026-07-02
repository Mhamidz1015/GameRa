using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Games.Infrastructure.Games;

internal sealed class GameRepository(GamesDbContext context) : IGameRepository
{
    public async Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public void Insert(Game game)
    {
        context.Games.Add(game);
    }
}
