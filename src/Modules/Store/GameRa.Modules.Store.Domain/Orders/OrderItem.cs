using GameRa.Modules.Store.Domain.Games;

namespace GameRa.Modules.Store.Domain.Orders;

public sealed class OrderItem
{
    private OrderItem()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid GameId { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    internal static OrderItem Create(Guid orderId,Game game, decimal unitPrice, string currency)
    {
        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            GameId = game.Id,
            UnitPrice = unitPrice,
            Currency = currency
        };

        return orderItem;
    }

}
