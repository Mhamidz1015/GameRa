using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;

namespace GameRa.Modules.Distribution.Infrastructure.Ownerships;

internal sealed class DapperGameOwnershipReader(IDbConnectionFactory dbConnectionFactory) : IGameOwnershipReader
{
    public async Task<bool> OwnsAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            SELECT EXISTS(
                SELECT 1
                FROM distribution.game_ownerships
                WHERE user_id = @UserId AND game_id = @GameId AND is_owned
            )
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, new { UserId = userId, GameId = gameId });
    }
}
