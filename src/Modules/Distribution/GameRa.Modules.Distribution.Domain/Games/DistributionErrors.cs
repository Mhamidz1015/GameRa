using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.Games;

public static class DistributionErrors
{
    public static Error GameNotFound(Guid gameId) => Error.NotFound(
        "Distribution.GameNotFound",
        $"The game with the identifier {gameId} was not found");

    public static Error GameNotOwned(Guid gameId) => Error.Forbidden(
        "Distribution.GameNotOwned",
        $"The current user does not own the game with the identifier {gameId}");
}
