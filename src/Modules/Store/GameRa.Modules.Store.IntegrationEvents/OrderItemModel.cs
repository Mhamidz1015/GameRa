namespace GameRa.Modules.Store.IntegrationEvents;

public sealed class OrderItemModel
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public Guid GameId { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal FinalPrice { get; init; }
}
