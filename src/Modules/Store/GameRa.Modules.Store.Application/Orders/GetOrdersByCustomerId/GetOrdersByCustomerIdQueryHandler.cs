using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using System.Data.Common;

namespace GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;

internal sealed class GetOrdersByCustomerIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrdersByCustomerIdQuery, IReadOnlyCollection<OrderSummaryResponse>>
{
    public async Task<Result<IReadOnlyCollection<OrderSummaryResponse>>> Handle(
        GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 o.id             AS {nameof(OrderSummaryResponse.Id)},
                 o.customer_id    AS {nameof(OrderSummaryResponse.CustomerId)},
                 o.status         AS {nameof(OrderSummaryResponse.Status)},
                 o.total_price    AS {nameof(OrderSummaryResponse.TotalPrice)},
                 o.created_at_utc AS {nameof(OrderSummaryResponse.CreatedAtUtc)},
                 COUNT(oi.id)     AS {nameof(OrderSummaryResponse.ItemCount)}
             FROM store.orders o
             LEFT JOIN store.order_items oi ON oi.order_id = o.id
             WHERE o.customer_id = @CustomerId
             GROUP BY o.id
             ORDER BY o.created_at_utc DESC
             """;

        IEnumerable<OrderSummaryResponse> orders =
            await connection.QueryAsync<OrderSummaryResponse>(sql, request);

        return orders.ToList();
    }
}