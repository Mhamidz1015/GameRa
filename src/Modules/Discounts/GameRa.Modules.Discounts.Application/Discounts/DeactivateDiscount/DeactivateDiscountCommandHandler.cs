using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Abstractions.Data;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.DeactivateDiscount;

internal sealed class DeactivateDiscountCommandHandler(
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeactivateDiscountCommand>
{
    public async Task<Result> Handle(DeactivateDiscountCommand request, CancellationToken cancellationToken)
    {
        Discount? discount = await discountRepository.GetAsync(request.DiscountId, cancellationToken);

        if (discount is null)
        {
            return Result.Failure(DiscountErrors.NotFound(request.DiscountId));
        }

        Result result = discount.Deactivate();

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}