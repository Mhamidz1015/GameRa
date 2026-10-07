using System.ComponentModel.DataAnnotations.Schema;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.GameVersions;

public sealed class GameVersion : Entity
{
    public const int MaxReleaseNotesLength = 4000;

    private readonly List<VersionFile> _files = [];

    private GameVersion()
    {
    }

    public Guid Id { get; private set; }

    public Guid GameId { get; private set; }

    public int Major { get; private set; }

    public int Minor { get; private set; }

    public int Patch { get; private set; }

    public GameVersionStatus Status { get; private set; }

    public string? ReleaseNotes { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? PublishedOnUtc { get; private set; }

    public DateTime? DeprecatedOnUtc { get; private set; }

    public IReadOnlyCollection<VersionFile> Files => _files.ToList();

    [NotMapped]
    public VersionNumber Number => VersionNumber.From(Major, Minor, Patch);

    [NotMapped]
    public long TotalSizeBytes => _files.Sum(f => f.SizeBytes);

    [NotMapped]
    public int FileCount => _files.Count;

    public static Result<GameVersion> CreateDraft(
        Guid gameId,
        VersionNumber number,
        string? releaseNotes,
        DateTime utcNow)
    {
        if (releaseNotes?.Length > MaxReleaseNotesLength)
        {
            return Result.Failure<GameVersion>(GameVersionErrors.ReleaseNotesTooLong);
        }

        var gameVersion = new GameVersion
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            Major = number.Major,
            Minor = number.Minor,
            Patch = number.Patch,
            Status = GameVersionStatus.Draft,
            ReleaseNotes = string.IsNullOrWhiteSpace(releaseNotes) ? null : releaseNotes,
            CreatedOnUtc = utcNow
        };

        return gameVersion;
    }

    public Result<VersionFile> AddFile(string relativePath, long sizeBytes, string sha256)
    {
        if (Status != GameVersionStatus.Draft)
        {
            return Result.Failure<VersionFile>(GameVersionErrors.InvalidStatusTransition(Status, Status));
        }

        Result<string> path = VersionFile.CleanPath(relativePath);

        if (path.IsFailure)
        {
            return Result.Failure<VersionFile>(path.Error);
        }

        if (sizeBytes <= 0 || !IsValidSha256(sha256))
        {
            return Result.Failure<VersionFile>(GameVersionErrors.InvalidFile);
        }

        string normalizedSha256 = sha256.ToLowerInvariant();
        string normalizedPath = VersionFile.NormalizePath(path.Value);

        VersionFile? existing = _files.SingleOrDefault(f => f.NormalizedPath == normalizedPath);

        if (existing is not null)
        {
            // Re-registering the same pending file is idempotent and gives the uploader a fresh upload URL.
            return existing.UploadStatus == FileUploadStatus.Pending &&
                   existing.SizeBytes == sizeBytes &&
                   existing.Sha256 == normalizedSha256
                ? existing
                : Result.Failure<VersionFile>(GameVersionErrors.FileAlreadyExists(path.Value));
        }

        var file = VersionFile.Create(GameId, Id, path.Value, sizeBytes, normalizedSha256);

        _files.Add(file);

        return file;
    }

    public Result RemoveFile(Guid fileId)
    {
        if (Status != GameVersionStatus.Draft)
        {
            return Result.Failure(GameVersionErrors.InvalidStatusTransition(Status, Status));
        }

        VersionFile? file = _files.SingleOrDefault(f => f.Id == fileId);

        if (file is null)
        {
            return Result.Failure(GameVersionErrors.FileNotFound(fileId));
        }

        _files.Remove(file);

        return Result.Success();
    }

    public Result MarkFileVerified(Guid fileId)
    {
        VersionFile? file = _files.SingleOrDefault(f => f.Id == fileId);

        if (file is null)
        {
            return Result.Failure(GameVersionErrors.FileNotFound(fileId));
        }

        if (file.UploadStatus == FileUploadStatus.Verified)
        {
            return Result.Success();
        }

        if (Status != GameVersionStatus.Draft)
        {
            return Result.Failure(GameVersionErrors.InvalidStatusTransition(Status, Status));
        }

        file.MarkVerified();

        return Result.Success();
    }

    public Result MarkReady()
    {
        if (Status == GameVersionStatus.Ready)
        {
            return Result.Success();
        }

        if (Status != GameVersionStatus.Draft)
        {
            return Result.Failure(GameVersionErrors.InvalidStatusTransition(Status, GameVersionStatus.Ready));
        }

        if (_files.Count == 0)
        {
            return Result.Failure(GameVersionErrors.NoFiles);
        }

        if (_files.Any(f => f.UploadStatus != FileUploadStatus.Verified))
        {
            return Result.Failure(GameVersionErrors.UnverifiedFiles);
        }

        Status = GameVersionStatus.Ready;

        return Result.Success();
    }

    public Result Publish(DateTime utcNow)
    {
        if (Status == GameVersionStatus.Published)
        {
            return Result.Success();
        }

        if (Status != GameVersionStatus.Ready)
        {
            return Result.Failure(GameVersionErrors.InvalidStatusTransition(Status, GameVersionStatus.Published));
        }

        Status = GameVersionStatus.Published;
        PublishedOnUtc = utcNow;

        Raise(new GameVersionPublishedDomainEvent(Id, GameId, Number.ToString()));

        return Result.Success();
    }

    public Result Deprecate(DateTime utcNow)
    {
        if (Status == GameVersionStatus.Deprecated)
        {
            return Result.Success();
        }

        if (Status != GameVersionStatus.Published)
        {
            return Result.Failure(GameVersionErrors.InvalidStatusTransition(Status, GameVersionStatus.Deprecated));
        }

        Status = GameVersionStatus.Deprecated;
        DeprecatedOnUtc = utcNow;

        Raise(new GameVersionDeprecatedDomainEvent(Id, GameId, Number.ToString()));

        return Result.Success();
    }

    private static bool IsValidSha256(string? sha256) =>
        sha256 is { Length: 64 } && sha256.All(char.IsAsciiHexDigit);
}
