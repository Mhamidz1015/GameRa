using FluentValidation;

namespace GameRa.Modules.Games.Application.Games.DelistGame;

internal sealed class DelistGameCommandValidator : AbstractValidator<DelistGameCommand>
{
    public DelistGameCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}
