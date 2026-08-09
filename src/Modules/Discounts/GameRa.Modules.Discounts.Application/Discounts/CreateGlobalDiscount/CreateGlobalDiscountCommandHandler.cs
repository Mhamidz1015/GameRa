using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Abstractions.Data;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateGlobalDiscount;

internal sealed class CreateGlobalDiscountCommandHandler(
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateGlobalDiscountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGlobalDiscountCommand request, CancellationToken cancellationToken)
    {
        bool codeExists = await discountRepository.ExistsByCodeAsync(request.Code, cancellationToken);

        if (codeExists)
        {
            return Result.Failure<Guid>(DiscountErrors.CodeAlreadyExists(request.Code));
        }

        Result<Discount> result = Discount.CreateGlobal(
            request.Code,
            request.Type,
            request.Amount,
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