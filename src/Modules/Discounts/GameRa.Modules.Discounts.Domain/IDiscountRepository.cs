namespace GameRa.Modules.Discounts.Domain;

public interface IDiscountRepository
{
    Task<Discount?> GetAsync(Guid discountId, CancellationToken cancellationToken = default);

    Task<Discount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<List<Discount>> GetActiveByGameIdAsync(Guid gameId, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<List<Discount>> GetActiveByCategoryIdAsync(Guid categoryId, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<List<Discount>> GetActiveGlobalAsync(DateTime utcNow, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    void Insert(Discount discount);
}