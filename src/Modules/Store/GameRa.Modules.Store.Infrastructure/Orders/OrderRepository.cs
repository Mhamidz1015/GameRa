using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Store.Infrastructure.Orders;

internal sealed class OrderRepository(StoreDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        context.Orders.Add(order);
    }
}
