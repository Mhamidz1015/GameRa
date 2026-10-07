using FluentValidation;

namespace GameRa.Modules.Distribution.Application.Games.AddDistributionGame;

internal sealed class AddDistributionGameCommandValidator : AbstractValidator<AddDistributionGameCommand>
{
    public AddDistributionGameCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}
