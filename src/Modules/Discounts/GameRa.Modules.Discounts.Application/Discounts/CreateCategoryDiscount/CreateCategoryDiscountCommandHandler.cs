using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Abstractions.Data;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateCategoryDiscount;

internal sealed class CreateCategoryDiscountCommandHandler(
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryDiscountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryDiscountCommand request, CancellationToken cancellationToken)
    {
        bool codeExists = await discountRepository.ExistsByCodeAsync(request.Code, cancellationToken);

        if (codeExists)
        {
            return Result.Failure<Guid>(DiscountErrors.CodeAlreadyExists(request.Code));
        }

        Result<Discount> result = Discount.CreateForCategory(
            request.Code,
            request.Type,
            request.Amount,
            request.CategoryId,
            request.StartDateTimeUtc,
            request.EndDateTimeUtc);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        discountRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.DiscountId;
    }
}