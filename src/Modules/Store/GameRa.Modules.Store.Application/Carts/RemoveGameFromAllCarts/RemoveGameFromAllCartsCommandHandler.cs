using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using System.Data.Common;

namespace GameRa.Modules.Store.Application.Carts.RemoveGameFromAllCarts;

internal sealed class RemoveGameFromAllCartsCommandHandler(
    IDbConnectionFactory dbConnectionFactory)
    : ICommandHandler<RemoveGameFromAllCartsCommand>
{
    public async Task<Result> Handle(
        RemoveGameFromAllCartsCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            INSERT INTO store.delisted_games (game_id, delisted_at_utc)
            VALUES (@GameId, @DelistedAtUtc)
            ON CONFLICT (game_id) DO NOTHING
            """;

        await connection.ExecuteAsync(sql, new
        {
            request.GameId,
            DelistedAtUtc = DateTime.UtcNow
        });

        return Result.Success();
    }
}