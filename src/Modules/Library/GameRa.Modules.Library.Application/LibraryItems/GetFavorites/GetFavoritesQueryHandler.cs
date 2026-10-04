using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using System.Data.Common;

namespace GameRa.Modules.Library.Application.LibraryItems.GetFavorites;

internal sealed class GetFavoritesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetFavoritesQuery, IReadOnlyCollection<LibraryItemResponse>>
{
    public async Task<Result<IReadOnlyCollection<LibraryItemResponse>>> Handle(
        GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 l.id AS {nameof(LibraryItemResponse.Id)},
                 l.game_id AS {nameof(LibraryItemResponse.GameId)},
                 l.user_id AS {nameof(LibraryItemResponse.UserId)},
                 l.gametitle_snapshot AS {nameof(LibraryItemResponse.GameTitleSnapshot)},
                 l.is_archived AS {nameof(LibraryItemResponse.IsArchived)},
                 l.is_favorite AS {nameof(LibraryItemResponse.IsFavorite)}
             FROM libraryitem.library_items l
             WHERE l.user_id = @UserId
               AND l.is_favorite = TRUE
               AND l.is_archived = FALSE
             ORDER BY l.gametitle_snapshot
             """;

        IEnumerable<LibraryItemResponse> items =
            await connection.QueryAsync<LibraryItemResponse>(sql, request);

        return items.ToList();
    }
}