namespace GameRa.Modules.Distribution.Application.GameVersions.GetGameVersions;

public sealed class GameVersionResponse
{
    public Guid Id { get; init; }

    public Guid GameId { get; init; }

    public string Version { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string? ReleaseNotes { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime? PublishedOnUtc { get; init; }

    public DateTime? DeprecatedOnUtc { get; init; }

    public int FileCount { get; init; }

    public int VerifiedFileCount { get; init; }

    public long TotalSizeBytes { get; init; }
}
