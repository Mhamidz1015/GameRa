using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.Games;

public sealed class DistributionGame : Entity
{
    private DistributionGame()
    {
    }

    public Guid GameId { get; private set; }

    public static DistributionGame Create(Guid gameId)
    {
        return new DistributionGame { GameId = gameId };
    }
}
