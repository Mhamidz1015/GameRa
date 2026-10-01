using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GameRa.Modules.Store.Application.Wishlist.GetWishlist;

internal sealed class GetWishlistQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetWishlistQuery, IReadOnlyCollection<WishlistItemResponse>>
{
    public async Task<Result<IReadOnlyCollection<WishlistItemResponse>>> Handle(
        GetWishlistQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 w.id          AS {nameof(WishlistItemResponse.Id)},
                 w.customer_id AS {nameof(WishlistItemResponse.CustomerId)},
                 w.game_id     AS {nameof(WishlistItemResponse.GameId)},
                 w.added_at_utc AS {nameof(WishlistItemResponse.AddedAtUtc)}
             FROM store.wishlist_items w
             WHERE w.customer_id = @CustomerId
             ORDER BY w.added_at_utc DESC
             """;

        IEnumerable<WishlistItemResponse> items =
            await connection.QueryAsync<WishlistItemResponse>(sql, request);

        return items.ToList();
    }
}