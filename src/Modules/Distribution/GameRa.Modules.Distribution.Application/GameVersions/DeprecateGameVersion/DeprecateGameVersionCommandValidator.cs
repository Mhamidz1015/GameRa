using FluentValidation;

namespace GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;

internal sealed class DeprecateGameVersionCommandValidator : AbstractValidator<DeprecateGameVersionCommand>
{
    public DeprecateGameVersionCommandValidator()
    {
        RuleFor(c => c.VersionId).NotEmpty();
    }
}
