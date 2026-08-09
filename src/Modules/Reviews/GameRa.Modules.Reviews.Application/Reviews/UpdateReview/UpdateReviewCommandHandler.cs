using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler(
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateReviewCommand>
{
    public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
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

        Result result = review.Update(request.Rating, request.Comment);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}