using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Reviews.Infrastructure.Reviews;

internal sealed class ReviewRepository(ReviewsDbContext context) : IReviewRepository
{
    public async Task<Review?> GetAsync(Guid reviewId, CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .SingleOrDefaultAsync(r => r.ReviewId == reviewId, cancellationToken);
    }

    public async Task<bool> ExistsByGameAndUserAsync(
        Guid gameId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Reviews
            .AnyAsync(r => r.GameId == gameId && r.UserId == userId, cancellationToken);
    }

    public void Insert(Review review)
    {
        context.Reviews.Add(review);
    }

    public void Remove(Review review)
    {
        context.Reviews.Remove(review);
    }
}
