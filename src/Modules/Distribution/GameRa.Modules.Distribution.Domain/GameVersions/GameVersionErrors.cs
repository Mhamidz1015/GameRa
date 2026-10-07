using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Domain.GameVersions;

public static class GameVersionErrors
{
    public static Error NotFound(Guid versionId) => Error.NotFound(
        "GameVersions.NotFound",
        $"The game version with the identifier {versionId} was not found");

    public static Error VersionAlreadyExists(Guid gameId, string version) => Error.Conflict(
        "GameVersions.VersionAlreadyExists",
        $"Version {version} already exists for the game with the identifier {gameId}");

    public static Error InvalidVersionNumber(string? version) => Error.Problem(
        "GameVersions.InvalidVersionNumber",
        $"'{version}' is not a valid version number (expected MAJOR.MINOR.PATCH)");

    public static readonly Error ReleaseNotesTooLong = Error.Problem(
        "GameVersions.ReleaseNotesTooLong",
        $"Release notes can not be longer than {GameVersion.MaxReleaseNotesLength} characters");

    public static Error InvalidStatusTransition(GameVersionStatus from, GameVersionStatus to) => Error.Problem(
        "GameVersions.InvalidStatusTransition",
        $"The operation is not allowed when moving the game version from {from} to {to}");

    public static Error FileAlreadyExists(string path) => Error.Conflict(
        "GameVersions.FileAlreadyExists",
        $"A file with the path '{path}' already exists in this version");

    public static Error InvalidPath(string? path) => Error.Problem(
        "GameVersions.InvalidPath",
        $"'{path}' is not a valid relative file path");

    public static readonly Error InvalidFile = Error.Problem(
        "GameVersions.InvalidFile",
        "The file size must be positive and the SHA-256 must be 64 hexadecimal characters");

    public static Error FileNotFound(Guid fileId) => Error.NotFound(
        "GameVersions.FileNotFound",
        $"The file with the identifier {fileId} was not found");

    public static readonly Error NoFiles = Error.Problem(
        "GameVersions.NoFiles",
        "The game version has no files");

    public static readonly Error UnverifiedFiles = Error.Problem(
        "GameVersions.UnverifiedFiles",
        "All files must be uploaded and verified first");

    public static Error UploadNotFound(Guid fileId) => Error.Problem(
        "GameVersions.UploadNotFound",
        $"The file with the identifier {fileId} has not been uploaded yet");

    public static Error UploadSizeMismatch(Guid fileId) => Error.Problem(
        "GameVersions.UploadSizeMismatch",
        $"The uploaded size of the file with the identifier {fileId} does not match the declared size");

    public static Error UploadChecksumMismatch(Guid fileId) => Error.Problem(
        "GameVersions.UploadChecksumMismatch",
        $"The uploaded SHA-256 of the file with the identifier {fileId} does not match the declared value");

    public static Error NoPublishedVersion(Guid gameId) => Error.NotFound(
        "GameVersions.NoPublishedVersion",
        $"The game with the identifier {gameId} has no published version");

    public static Error NotDownloadable(Guid fileId) => Error.NotFound(
        "GameVersions.NotDownloadable",
        $"The file with the identifier {fileId} was not found");
}
