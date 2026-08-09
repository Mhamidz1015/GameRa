using FluentValidation;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateCategoryDiscount;

internal sealed class CreateCategoryDiscountCommandValidator : AbstractValidator<CreateCategoryDiscountCommand>
{
    public CreateCategoryDiscountCommandValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Type)
            .IsInEnum();

        RuleFor(c => c.Amount)
            .GreaterThan(0);

        RuleFor(c => c.Amount)
            .LessThanOrEqualTo(100)
            .When(c => c.Type == DiscountType.Percentage)
            .WithMessage("Percentage discount cannot exceed 100%.");

        RuleFor(c => c.CategoryId)
            .NotEmpty();

        RuleFor(c => c.StartDateTimeUtc)
            .NotEmpty();

        RuleFor(c => c.EndDateTimeUtc)
            .NotEmpty()
            .GreaterThan(c => c.StartDateTimeUtc)
            .WithMessage("EndDateTimeUtc must be later than StartDateTimeUtc.");
    }
}