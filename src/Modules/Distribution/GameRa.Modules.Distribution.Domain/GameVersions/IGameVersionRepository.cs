namespace GameRa.Modules.Distribution.Domain.GameVersions;

public interface IGameVersionRepository
{
    Task<GameVersion?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid gameId, VersionNumber number, CancellationToken cancellationToken = default);

    void Insert(GameVersion gameVersion);
}
