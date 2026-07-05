using GameRa.Modules.Store.Domain.Games;

namespace GameRa.Modules.Store.Domain.Payments;

public interface IPaymentRepository
{
    Task<Payment?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Payment>> GetForGameAsync(Game game, CancellationToken cancellationToken = default);

    void Insert(Payment payment);
}
