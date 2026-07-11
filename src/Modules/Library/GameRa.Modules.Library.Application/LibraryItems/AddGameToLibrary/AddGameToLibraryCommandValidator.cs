using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;

internal sealed class AddGameToLibraryCommandValidator : AbstractValidator<AddGameToLibraryCommand>
{
    public AddGameToLibraryCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
        RuleFor(c => c.GameTitleSnapshot).NotEmpty();
    }
}
