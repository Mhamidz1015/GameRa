using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.GameVersions;

public sealed class VersionFile : Entity
{
    public const int MaxPathLength = 1024;

    private VersionFile()
    {
    }

    public Guid Id { get; private set; }

    public Guid GameVersionId { get; private set; }

    public string RelativePath { get; private set; } = string.Empty;

    public string NormalizedPath { get; private set; } = string.Empty;

    public long SizeBytes { get; private set; }

    public string Sha256 { get; private set; } = string.Empty;

    public string StorageKey { get; private set; } = string.Empty;

    public FileUploadStatus UploadStatus { get; private set; }

    internal static VersionFile Create(
        Guid gameId,
        Guid gameVersionId,
        string relativePath,
        long sizeBytes,
        string sha256)
    {
        var fileId = Guid.NewGuid();

        return new VersionFile
        {
            Id = fileId,
            GameVersionId = gameVersionId,
            RelativePath = relativePath,
            NormalizedPath = NormalizePath(relativePath),
            SizeBytes = sizeBytes,
            Sha256 = sha256,
            StorageKey = $"games/{gameId}/versions/{gameVersionId}/{fileId}",
            UploadStatus = FileUploadStatus.Pending
        };
    }

    internal static Result<string> CleanPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Result.Failure<string>(GameVersionErrors.InvalidPath(path));
        }

        string cleaned = path.Trim().Replace('\\', '/');

        if (cleaned.Length > MaxPathLength ||
            cleaned.StartsWith('/') ||
            cleaned.Contains(':') ||
            cleaned.Any(char.IsControl) ||
            cleaned.Split('/').Any(segment => segment.Length == 0 || segment == "." || segment == ".."))
        {
            return Result.Failure<string>(GameVersionErrors.InvalidPath(path));
        }

        return cleaned;
    }

    internal static string NormalizePath(string cleanedPath) => cleanedPath.ToLowerInvariant();

    internal void MarkVerified()
    {
        UploadStatus = FileUploadStatus.Verified;
    }
}
