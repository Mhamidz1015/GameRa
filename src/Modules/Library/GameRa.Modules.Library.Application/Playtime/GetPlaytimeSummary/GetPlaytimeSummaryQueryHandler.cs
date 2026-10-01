using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using System.Data.Common;

namespace GameRa.Modules.Library.Application.Playtime.GetPlaytimeSummary;

internal sealed class GetPlaytimeSummaryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetPlaytimeSummaryQuery, IReadOnlyCollection<PlaytimeSummaryResponse>>
{
    public async Task<Result<IReadOnlyCollection<PlaytimeSummaryResponse>>> Handle(
        GetPlaytimeSummaryQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 p.game_id        AS {nameof(PlaytimeSummaryResponse.GameId)},
                 p.total_minutes  AS {nameof(PlaytimeSummaryResponse.TotalMinutes)},
                 p.last_played_utc AS {nameof(PlaytimeSummaryResponse.LastPlayedUtc)}
             FROM libraryitem.playtime_records p
             WHERE p.user_id = @UserId
             ORDER BY p.total_minutes DESC
             """;

        IEnumerable<PlaytimeSummaryResponse> records =
            await connection.QueryAsync<PlaytimeSummaryResponse>(sql, request);

        return records.ToList();
    }
}