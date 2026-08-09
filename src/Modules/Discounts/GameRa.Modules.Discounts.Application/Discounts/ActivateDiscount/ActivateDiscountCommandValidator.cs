using FluentValidation;

namespace GameRa.Modules.Discounts.Application.Discounts.ActivateDiscount;

internal sealed class ActivateDiscountCommandValidator : AbstractValidator<ActivateDiscountCommand>
{
    public ActivateDiscountCommandValidator()
    {
        RuleFor(c => c.DiscountId).NotEmpty();
    }
}