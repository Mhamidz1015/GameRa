using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.RemoveGameFromLibrary;

internal sealed class RemoveGameFromLibraryCommandValidator : AbstractValidator<RemoveGameFromLibraryCommand>
{
    public RemoveGameFromLibraryCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}
