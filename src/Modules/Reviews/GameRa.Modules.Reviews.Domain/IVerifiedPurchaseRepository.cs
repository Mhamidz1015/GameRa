
namespace GameRa.Modules.Reviews.Domain
{
    public interface IVerifiedPurchaseRepository
    {
        Task<bool> ExistsAsync(Guid gameId, Guid userId, CancellationToken cancellationToken = default);
        void Insert(VerifiedPurchase purchase);
    }
}
