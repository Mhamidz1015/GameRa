using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Application.GameVersions.GetGameVersions;

internal sealed class GetGameVersionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetGameVersionsQuery, IReadOnlyCollection<GameVersionResponse>>
{
    public async Task<Result<IReadOnlyCollection<GameVersionResponse>>> Handle(
        GetGameVersionsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 v.id AS {nameof(GameVersionResponse.Id)},
                 v.game_id AS {nameof(GameVersionResponse.GameId)},
                 concat(v.major, '.', v.minor, '.', v.patch) AS {nameof(GameVersionResponse.Version)},
                 v.status AS {nameof(GameVersionResponse.Status)},
                 v.release_notes AS {nameof(GameVersionResponse.ReleaseNotes)},
                 v.created_on_utc AS {nameof(GameVersionResponse.CreatedOnUtc)},
                 v.published_on_utc AS {nameof(GameVersionResponse.PublishedOnUtc)},
                 v.deprecated_on_utc AS {nameof(GameVersionResponse.DeprecatedOnUtc)},
                 count(f.id)::int AS {nameof(GameVersionResponse.FileCount)},
                 count(f.id) FILTER (WHERE f.upload_status = 'Verified')::int AS {nameof(GameVersionResponse.VerifiedFileCount)},
                 coalesce(sum(f.size_bytes), 0)::bigint AS {nameof(GameVersionResponse.TotalSizeBytes)}
             FROM distribution.game_versions v
             LEFT JOIN distribution.version_files f ON f.game_version_id = v.id
             WHERE v.game_id = @GameId
             GROUP BY v.id
             ORDER BY v.major DESC, v.minor DESC, v.patch DESC
             """;

        IReadOnlyCollection<GameVersionResponse> versions =
            (await connection.QueryAsync<GameVersionResponse>(sql, new { request.GameId })).AsList();

        return Result.Success(versions);
    }
}
