using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.UnarchiveLibraryItem;

internal sealed class UnarchiveLibraryItemCommandValidator
    : AbstractValidator<UnarchiveLibraryItemCommand>
{
    public UnarchiveLibraryItemCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}