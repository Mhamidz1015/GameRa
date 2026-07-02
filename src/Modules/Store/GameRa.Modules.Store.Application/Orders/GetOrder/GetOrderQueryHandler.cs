using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Orders;

namespace GameRa.Modules.Store.Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrderQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 o.id AS {nameof(OrderResponse.Id)},
                 o.customer_id AS {nameof(OrderResponse.CustomerId)},
                 o.status AS {nameof(OrderResponse.Status)},
                 o.total_price AS {nameof(OrderResponse.TotalPrice)},
                 o.created_at_utc AS {nameof(OrderResponse.CreatedAtUtc)},
                 oi.id AS {nameof(OrderItemResponse.OrderItemId)},
                 oi.order_id AS {nameof(OrderItemResponse.OrderId)},
                 oi.game_id AS {nameof(OrderItemResponse.GameId)},
                 oi.unit_price AS {nameof(OrderItemResponse.UnitPrice)},
                 oi.price AS {nameof(OrderItemResponse.Price)},
                 oi.currency AS {nameof(OrderItemResponse.Currency)}
             FROM ticketing.orders o
             JOIN ticketing.order_items oi ON oi.order_id = o.id
             WHERE o.id = @OrderId
             """;

        Dictionary<Guid, OrderResponse> ordersDictionary = [];
        await connection.QueryAsync<OrderResponse, OrderItemResponse, OrderResponse>(
            sql,
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out OrderResponse? existingEvent))
                {
                    order = existingEvent;
                }
                else
                {
                    ordersDictionary.Add(order.Id, order);
                }

                order.OrderItems.Add(orderItem);

                return order;
            },
            request,
            splitOn: nameof(OrderItemResponse.OrderItemId));

        if (!ordersDictionary.TryGetValue(request.OrderId, out OrderResponse orderResponse))
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound(request.OrderId));
        }

        return orderResponse;
    }
}
