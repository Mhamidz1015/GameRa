using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.CreateReview;

internal sealed class CreateReviewCommandHandler(
    IReviewRepository reviewRepository,
    IVerifiedPurchaseRepository verifiedPurchaseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        bool alreadyReviewed = await reviewRepository.ExistsByGameAndUserAsync(
            request.GameId,
            request.UserId,
            cancellationToken);

        if (alreadyReviewed)
        {
            return Result.Failure<Guid>(ReviewErrors.DuplicateReview(request.GameId));
        }

        bool isVerified = await verifiedPurchaseRepository.ExistsAsync(
        request.GameId,
        request.UserId,
        cancellationToken);

        Result<Review> result = Review.Create(
            request.GameId, request.UserId,
            request.Rating, request.Comment,
            isVerified);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        reviewRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.ReviewId;
    }
}