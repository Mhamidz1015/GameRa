namespace GameRa.Modules.Distribution.Application.Downloads.GetVersionManifest;

public sealed class VersionManifestResponse
{
    public Guid VersionId { get; init; }

    public Guid GameId { get; init; }

    public string Version { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string? ReleaseNotes { get; init; }

    public DateTime? PublishedOnUtc { get; init; }

    public int FileCount { get; set; }

    public long TotalSizeBytes { get; set; }

    public IReadOnlyCollection<ManifestFileResponse> Files { get; set; } = [];
}

public sealed class ManifestFileResponse
{
    public Guid FileId { get; init; }

    public string RelativePath { get; init; } = string.Empty;

    public long SizeBytes { get; init; }

    public string Sha256 { get; init; } = string.Empty;
}
