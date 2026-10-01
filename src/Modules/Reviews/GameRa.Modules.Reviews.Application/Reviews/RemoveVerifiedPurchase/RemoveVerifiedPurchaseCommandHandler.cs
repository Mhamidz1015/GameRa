using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.RemoveVerifiedPurchase;

internal sealed class RemoveVerifiedPurchaseCommandHandler(
    IVerifiedPurchaseRepository verifiedPurchaseRepository,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveVerifiedPurchaseCommand>
{
    public async Task<Result> Handle(
        RemoveVerifiedPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        VerifiedPurchase? purchase = await verifiedPurchaseRepository.GetAsync(
            request.GameId,
            request.UserId,
            cancellationToken);

        if (purchase is not null)
        {
            verifiedPurchaseRepository.Remove(purchase);
        }

        Review? review = await reviewRepository.GetByGameAndUserAsync(
            request.GameId,
            request.UserId,
            cancellationToken);

        review?.RevokeVerifiedPurchase();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
