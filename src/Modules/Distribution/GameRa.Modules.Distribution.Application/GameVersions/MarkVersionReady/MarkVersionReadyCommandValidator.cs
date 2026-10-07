using FluentValidation;

namespace GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;

internal sealed class MarkVersionReadyCommandValidator : AbstractValidator<MarkVersionReadyCommand>
{
    public MarkVersionReadyCommandValidator()
    {
        RuleFor(c => c.VersionId).NotEmpty();
    }
}
