using FluentValidation;

namespace GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;

internal sealed class ApplyGameOwnershipCommandValidator : AbstractValidator<ApplyGameOwnershipCommand>
{
    public ApplyGameOwnershipCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
        RuleFor(c => c.OccurredOnUtc).NotEmpty();
    }
}
