using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.GameVersions;

public sealed class GameVersionDeprecatedDomainEvent(Guid versionId, Guid gameId, string version) : DomainEvent
{
    public Guid VersionId { get; init; } = versionId;

    public Guid GameId { get; init; } = gameId;

    public string Version { get; init; } = version;
}
