using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;

internal sealed class ArchiveLibraryItemCommandValidator
    : AbstractValidator<ArchivedLibraryItemCommand>
{
    public ArchiveLibraryItemCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
    }
}
