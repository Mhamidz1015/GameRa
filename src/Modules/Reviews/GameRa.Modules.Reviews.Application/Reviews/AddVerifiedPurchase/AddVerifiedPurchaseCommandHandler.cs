using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Abstractions.Data;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.AddVerifiedPurchase;

internal sealed class AddVerifiedPurchaseCommandHandler(
    IVerifiedPurchaseRepository verifiedPurchaseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddVerifiedPurchaseCommand>
{
    public async Task<Result> Handle(
        AddVerifiedPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        bool alreadyExists = await verifiedPurchaseRepository.ExistsAsync(
            request.GameId,
            request.UserId,
            cancellationToken);

        if (alreadyExists)
            return Result.Success();

        VerifiedPurchase verifiedPurchase = VerifiedPurchase.Create(
            request.GameId,
            request.UserId,
            request.PurchasedAtUtc);

        verifiedPurchaseRepository.Insert(verifiedPurchase);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}