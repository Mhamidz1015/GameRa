using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.Infrastructure.Database;

namespace GameRa.Modules.Games.Infrastructure.Games;

internal sealed class GameRepository(GamesDbContext context) : IGameRepository
{
    public void Insert(Game game)
    {
        context.Games.Add(game);
    }
}
