namespace GameRa.Modules.Distribution.Domain.Games;

public interface IDistributionGameRepository
{
    Task<bool> ExistsAsync(Guid gameId, CancellationToken cancellationToken = default);

    void Insert(DistributionGame game);
}
