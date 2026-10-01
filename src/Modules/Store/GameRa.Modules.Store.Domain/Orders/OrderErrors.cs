using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Orders;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found");

    public static readonly Error OrderHasIssues = Error.Problem(
        "Order.ordersHasIssued",
        "The orders for this order were already issued");

    public static readonly Error NotCompleted =
        Error.Problem("Orders.NotCompleted", "The Order is not NotCompleted");

    public static readonly Error AlreadyRefunded = Error.Problem(
        "Orders.AlreadyRefunded",
        "The order was already Refunded");
}
