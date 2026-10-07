namespace GameRa.Modules.Distribution.Domain.Ownerships;

public interface IGameOwnershipRepository
{
    Task<GameOwnership?> GetAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);

    void Insert(GameOwnership gameOwnership);
}
