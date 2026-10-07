namespace GameRa.Modules.Distribution.Application.Downloads;

public sealed class FileDownloadUrlResponse
{
    public Guid FileId { get; init; }

    public string RelativePath { get; init; } = string.Empty;

    public long SizeBytes { get; init; }

    public string Sha256 { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;

    public DateTime ExpiresAtUtc { get; init; }
}
