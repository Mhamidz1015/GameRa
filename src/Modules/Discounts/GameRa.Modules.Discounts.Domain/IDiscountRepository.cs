
namespace GameRa.Modules.Discounts.Domain
{
    public interface IDiscountRepository
    {
        Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Discount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        void Add(Discount discount);

        void Update(Discount discount);

        void Remove(Discount discount);
    }
}
