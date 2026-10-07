using FluentValidation;

namespace GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;

internal sealed class RevokeGameFromAllLibrariesCommandValidator
    : AbstractValidator<RevokeGameFromAllLibrariesCommand>
{
    public RevokeGameFromAllLibrariesCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
    }
}