using GameRa.Modules.Store.Domain.Orders;

namespace GameRa.Modules.Store.Application.Orders.GetOrder;

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAtUtc)
{
    public List<OrderItemResponse> OrderItems { get; } = [];
}
