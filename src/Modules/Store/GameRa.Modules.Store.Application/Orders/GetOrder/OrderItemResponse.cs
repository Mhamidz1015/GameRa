namespace GameRa.Modules.Store.Application.Orders.GetOrder;

public sealed record OrderItemResponse(
    Guid OrderItemId,
    Guid OrderId,
    Guid GameId,
    decimal UnitPrice,
    decimal Price);
