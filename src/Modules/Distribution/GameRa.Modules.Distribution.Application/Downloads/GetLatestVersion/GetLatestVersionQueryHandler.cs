using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Caching;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Caching;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;

internal sealed class GetLatestVersionQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IGameOwnershipReader ownershipReader,
    ICacheService cacheService)
    : IQueryHandler<GetLatestVersionQuery, LatestVersionResponse>
{
    public async Task<Result<LatestVersionResponse>> Handle(
        GetLatestVersionQuery request,
        CancellationToken cancellationToken)
    {
        // Ownership is always checked first and never cached.
        if (!await ownershipReader.OwnsAsync(request.UserId, request.GameId, cancellationToken))
        {
            return Result.Failure<LatestVersionResponse>(DistributionErrors.GameNotOwned(request.GameId));
        }

        string cacheKey = DistributionCacheKeys.LatestVersion(request.GameId);

        LatestVersionResponse? cached = await cacheService.GetAsync<LatestVersionResponse>(cacheKey, cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 v.id AS {nameof(LatestVersionResponse.VersionId)},
                 v.game_id AS {nameof(LatestVersionResponse.GameId)},
                 concat(v.major, '.', v.minor, '.', v.patch) AS {nameof(LatestVersionResponse.Version)},
                 v.release_notes AS {nameof(LatestVersionResponse.ReleaseNotes)},
                 v.published_on_utc AS {nameof(LatestVersionResponse.PublishedOnUtc)},
                 (SELECT count(*)::int FROM distribution.version_files f WHERE f.game_version_id = v.id) AS {nameof(LatestVersionResponse.FileCount)},
                 (SELECT coalesce(sum(f.size_bytes), 0)::bigint FROM distribution.version_files f WHERE f.game_version_id = v.id) AS {nameof(LatestVersionResponse.TotalSizeBytes)}
             FROM distribution.game_versions v
             WHERE v.game_id = @GameId AND v.status = 'Published'
             ORDER BY v.major DESC, v.minor DESC, v.patch DESC
             LIMIT 1
             """;

        LatestVersionResponse? latest =
            await connection.QuerySingleOrDefaultAsync<LatestVersionResponse>(sql, new { request.GameId });

        if (latest is null)
        {
            return Result.Failure<LatestVersionResponse>(GameVersionErrors.NoPublishedVersion(request.GameId));
        }

        await cacheService.SetAsync(cacheKey, latest, DistributionCacheKeys.LatestVersionExpiration, cancellationToken);

        return latest;
    }
}
