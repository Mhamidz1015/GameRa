using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.DeleteReview;

internal sealed class DeleteReviewCommandHandler(
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteReviewCommand>
{
    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        Review? review = await reviewRepository.GetAsync(request.ReviewId, cancellationToken);

        if (review is null)
        {
            return Result.Failure(ReviewErrors.NotFound(request.ReviewId));
        }

        if (review.UserId != request.UserId)
        {
            return Result.Failure(ReviewErrors.Forbidden);
        }

        review.Delete();

        reviewRepository.Remove(review);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}