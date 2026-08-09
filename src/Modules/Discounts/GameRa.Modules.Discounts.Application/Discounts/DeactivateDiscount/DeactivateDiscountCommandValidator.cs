using FluentValidation;

namespace GameRa.Modules.Discounts.Application.Discounts.DeactivateDiscount;

internal sealed class DeactivateDiscountCommandValidator : AbstractValidator<DeactivateDiscountCommand>
{
    public DeactivateDiscountCommandValidator()
    {
        RuleFor(c => c.DiscountId).NotEmpty();
    }
}