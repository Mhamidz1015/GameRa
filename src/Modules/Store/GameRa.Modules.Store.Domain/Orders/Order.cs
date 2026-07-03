using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Domain.Games;

namespace GameRa.Modules.Store.Domain.Orders;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _orderItems = [];

    private Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public string Currency { get; private set; }

    public bool OrderCompleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();

    public static Order Create(Customer customer)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };

        order.Raise(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddItem(Game game, decimal price, string currency)
    {
        var orderItem = OrderItem.Create(Id, game, price, currency);

        _orderItems.Add(orderItem);

        TotalPrice = _orderItems.Sum(o => o.Price);
        Currency = currency;
    }

    public Result CompleteOrder()
    {
        if (OrderCompleted)
        {
            return Result.Failure(OrderErrors.OrderHasIssues);
        }

        OrderCompleted = true;

        Raise(new OrderCompletedDomainEvent(Id));

        return Result.Success();
    }
}
