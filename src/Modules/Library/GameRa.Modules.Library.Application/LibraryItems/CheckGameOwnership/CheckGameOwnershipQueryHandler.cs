using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using MediatR;
using System.Data.Common;

namespace GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;

internal sealed class CheckGameOwnershipQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<CheckGameOwnershipQuery, bool>
{
    public async Task<Result<bool>> Handle(CheckGameOwnershipQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
        """
        SELECT EXISTS
        (
            SELECT 1
            FROM libraryitem.library_items
            WHERE user_id = @UserId
              AND game_id = @GameId
              AND is_archived = false
        );
        """;

        bool ownsGame = await connection.ExecuteScalarAsync<bool>(sql, request);

        return Result.Success(ownsGame);
    }
}
