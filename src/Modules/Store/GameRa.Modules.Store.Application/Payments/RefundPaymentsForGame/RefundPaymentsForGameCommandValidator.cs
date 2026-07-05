using FluentValidation;

namespace GameRa.Modules.Store.Application.Payments.RefundPaymentsForGame;

internal sealed class RefundPaymentsForGameCommandValidator : AbstractValidator<RefundPaymentsForGameCommand>
{
    public RefundPaymentsForGameCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}
