using FluentValidation;

namespace GameRa.Modules.Store.Application.Games.AddGame;

internal sealed class AddGameCommandValidator : AbstractValidator<AddGameCommand>
{
    public AddGameCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.Developer).NotEmpty();
        RuleFor(c => c.ReleasedDate).NotEmpty();
    }
}
