using FluentValidation;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class AddGameCommandValidator : AbstractValidator<AddGameCommand>
{
    public AddGameCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.Developer).NotEmpty();
        RuleFor(c => c.BasePrice).GreaterThan(0);
        RuleFor(c => c.CoverImageUrl).NotEmpty();
        RuleFor(c => c.ReleaseDate).NotEmpty();
    }
}
