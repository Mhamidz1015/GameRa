
namespace GameRa.Modules.Reviews.Domain;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Review>> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default);

    void Add(Review review);

    void Update(Review review);

    void Remove(Review review);
}