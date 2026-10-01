using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.ToggleFavorite;

internal sealed class ToggleFavoriteCommandValidator : AbstractValidator<ToggleFavoriteCommand>
{
    public ToggleFavoriteCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}