namespace GameRa.Modules.Reviews.Domain;

public interface IReviewRepository
{
    Task<Review?> GetAsync(Guid reviewId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByGameAndUserAsync(Guid gameId, Guid userId, CancellationToken cancellationToken = default);

    void Insert(Review review);

    void Remove(Review review);
}