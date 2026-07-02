using FluentValidation;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class AddGameCommandValidator : AbstractValidator<AddGameCommand>
{
    public AddGameCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Description).NotEmpty().MaximumLength(2000);
        RuleFor(c => c.Developer).NotEmpty().MaximumLength(100);
        RuleFor(c => c.BasePrice).GreaterThan(0);
        RuleFor(c => c.CoverImageUrl).NotEmpty();
        RuleFor(c => c.ReleaseDate).NotEmpty();
    }
}
