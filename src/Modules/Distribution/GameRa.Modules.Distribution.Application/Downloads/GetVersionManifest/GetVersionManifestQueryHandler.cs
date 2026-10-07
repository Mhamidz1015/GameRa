using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.Downloads.GetVersionManifest;

internal sealed class GetVersionManifestQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IGameOwnershipReader ownershipReader)
    : IQueryHandler<GetVersionManifestQuery, VersionManifestResponse>
{
    public async Task<Result<VersionManifestResponse>> Handle(
        GetVersionManifestQuery request,
        CancellationToken cancellationToken)
    {
        if (!await ownershipReader.OwnsAsync(request.UserId, request.GameId, cancellationToken))
        {
            return Result.Failure<VersionManifestResponse>(DistributionErrors.GameNotOwned(request.GameId));
        }

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Draft and Ready versions are invisible to players.
        const string versionSql =
            $"""
             SELECT
                 id AS {nameof(VersionManifestResponse.VersionId)},
                 game_id AS {nameof(VersionManifestResponse.GameId)},
                 concat(major, '.', minor, '.', patch) AS {nameof(VersionManifestResponse.Version)},
                 status AS {nameof(VersionManifestResponse.Status)},
                 release_notes AS {nameof(VersionManifestResponse.ReleaseNotes)},
                 published_on_utc AS {nameof(VersionManifestResponse.PublishedOnUtc)}
             FROM distribution.game_versions
             WHERE id = @VersionId AND game_id = @GameId AND status IN ('Published', 'Deprecated')
             """;

        VersionManifestResponse? manifest = await connection.QuerySingleOrDefaultAsync<VersionManifestResponse>(
            versionSql,
            new { request.VersionId, request.GameId });

        if (manifest is null)
        {
            return Result.Failure<VersionManifestResponse>(GameVersionErrors.NotFound(request.VersionId));
        }

        const string filesSql =
            $"""
             SELECT
                 id AS {nameof(ManifestFileResponse.FileId)},
                 relative_path AS {nameof(ManifestFileResponse.RelativePath)},
                 size_bytes AS {nameof(ManifestFileResponse.SizeBytes)},
                 sha256 AS {nameof(ManifestFileResponse.Sha256)}
             FROM distribution.version_files
             WHERE game_version_id = @VersionId
             ORDER BY normalized_path
             """;

        List<ManifestFileResponse> files =
            (await connection.QueryAsync<ManifestFileResponse>(filesSql, new { request.VersionId })).AsList();

        manifest.Files = files;
        manifest.FileCount = files.Count;
        manifest.TotalSizeBytes = files.Sum(f => f.SizeBytes);

        return manifest;
    }
}
