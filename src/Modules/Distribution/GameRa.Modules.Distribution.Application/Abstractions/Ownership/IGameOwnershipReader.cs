namespace GameRa.Modules.Distribution.Application.Abstractions.Ownership;

public interface IGameOwnershipReader
{
    Task<bool> OwnsAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);
}
