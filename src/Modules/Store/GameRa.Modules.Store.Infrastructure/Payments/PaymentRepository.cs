using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Payments;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Store.Infrastructure.Payments;

internal sealed class PaymentRepository(StoreDbContext context) : IPaymentRepository
{
    public async Task<Payment?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Payments.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetForEventAsync(
        Game game,
        CancellationToken cancellationToken = default)
    {
        return await (
            from order in context.Orders
            join payment in context.Payments on order.Id equals payment.OrderId
            join orderItem in context.OrderItems on order.Id equals orderItem.OrderId
            where orderItem.GameId == game.Id
            select payment).ToListAsync(cancellationToken);
    }

    public void Insert(Payment payment)
    {
        context.Payments.Add(payment);
    }
}
