using FluentValidation;

namespace GameRa.Modules.Reviews.Application.Reviews.RemoveVerifiedPurchase;

internal sealed class RemoveVerifiedPurchaseCommandValidator : AbstractValidator<RemoveVerifiedPurchaseCommand>
{
    public RemoveVerifiedPurchaseCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
    }
}
