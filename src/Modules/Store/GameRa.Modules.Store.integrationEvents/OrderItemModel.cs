namespace GameRa.Modules.Store.integrationEvents;

public sealed class OrderItemModel
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal Price { get; init; }

    public string Currency { get; init; }
}
