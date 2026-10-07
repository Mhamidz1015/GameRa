namespace GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;

public sealed class LatestVersionResponse
{
    public Guid VersionId { get; init; }

    public Guid GameId { get; init; }

    public string Version { get; init; } = string.Empty;

    public string? ReleaseNotes { get; init; }

    public DateTime? PublishedOnUtc { get; init; }

    public int FileCount { get; init; }

    public long TotalSizeBytes { get; init; }
}
