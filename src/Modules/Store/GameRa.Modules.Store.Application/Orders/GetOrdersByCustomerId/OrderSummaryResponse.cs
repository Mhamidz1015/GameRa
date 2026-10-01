namespace GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;

public sealed class OrderSummaryResponse
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public int Status { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public int ItemCount { get; init; }
}