using FluentValidation;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

internal sealed class ReleaseGameCommandValidator : AbstractValidator<ReleaseGameCommand>
{
    public ReleaseGameCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}
