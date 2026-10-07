using System.Data.Common;
using Dapper;

namespace GameRa.Modules.Distribution.Application.Downloads;

internal static class DownloadableFiles
{
    internal static async Task<IReadOnlyCollection<DownloadableFile>> GetAsync(
        DbConnection connection,
        Guid[] fileIds)
    {
        const string sql =
            $"""
             SELECT
                 f.id AS {nameof(DownloadableFile.FileId)},
                 v.game_id AS {nameof(DownloadableFile.GameId)},
                 v.status AS {nameof(DownloadableFile.VersionStatus)},
                 f.relative_path AS {nameof(DownloadableFile.RelativePath)},
                 f.size_bytes AS {nameof(DownloadableFile.SizeBytes)},
                 f.sha256 AS {nameof(DownloadableFile.Sha256)},
                 f.storage_key AS {nameof(DownloadableFile.StorageKey)}
             FROM distribution.version_files f
             JOIN distribution.game_versions v ON v.id = f.game_version_id
             WHERE f.id = ANY(@FileIds)
             """;

        return (await connection.QueryAsync<DownloadableFile>(sql, new { FileIds = fileIds })).AsList();
    }
}

internal sealed class DownloadableFile
{
    public Guid FileId { get; init; }

    public Guid GameId { get; init; }

    public string VersionStatus { get; init; } = string.Empty;

    public string RelativePath { get; init; } = string.Empty;

    public long SizeBytes { get; init; }

    public string Sha256 { get; init; } = string.Empty;

    public string StorageKey { get; init; } = string.Empty;

    // Installed copies of deprecated versions must still be able to verify and repair themselves.
    public bool IsDownloadable => VersionStatus is "Published" or "Deprecated";
}
