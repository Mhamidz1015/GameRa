using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions;
using MediatR;
using System.Data.Common;

namespace GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;

internal sealed class GetUserLibraryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserLibraryQuery, IReadOnlyCollection<LibraryItemResponse>>
{
    public async Task<Result<IReadOnlyCollection<LibraryItemResponse>>> Handle(
        GetUserLibraryQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql = """
        SELECT
            id AS Id,
            game_id AS GameId,
            gametitle_snapshot AS GameTitleSnapshot,
            is_archived AS IsArchived
        FROM libraryitem.library_items
        WHERE user_id = @UserId
        """;

        switch (request.Filter)
        {
            case LibraryFilter.Active:
                sql += " AND is_archived = false";
                break;
            case LibraryFilter.Archived:
                sql += " AND is_archived = true";
                break;
            case LibraryFilter.All:
                break;
        }

        IReadOnlyCollection<LibraryItemResponse> items =(
            await connection.QueryAsync<LibraryItemResponse>(
                sql,
                new { request.UserId })
        ).AsList();

        return Result.Success(items);
    }
}
