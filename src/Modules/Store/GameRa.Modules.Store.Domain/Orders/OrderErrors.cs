using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Orders;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found");


    public static readonly Error TicketsHasIssues = Error.Problem(
        "Order.TicketsHasIssued",
        "The tickets for this order were already issued");
}
