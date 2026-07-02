using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Games;

public static class GameErrors
{
    public static Error NotFound(Guid gameId) =>
        Error.NotFound("Games.NotFound", $"The game with the identifier {gameId} was not found");
}
