using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Abstractions.Data;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateGameDiscount;

internal sealed class CreateGameDiscountCommandHandler(
    IDiscountRepository discountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateGameDiscountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGameDiscountCommand request, CancellationToken cancellationToken)
    {
        bool codeExists = await discountRepository.ExistsByCodeAsync(request.Code, cancellationToken);

        if (codeExists)
        {
            return Result.Failure<Guid>(DiscountErrors.CodeAlreadyExists(request.Code));
        }

        Result<Discount> result = Discount.CreateForGame(
            request.Code,
            request.Type,
            request.Amount,
            request.GameId,
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