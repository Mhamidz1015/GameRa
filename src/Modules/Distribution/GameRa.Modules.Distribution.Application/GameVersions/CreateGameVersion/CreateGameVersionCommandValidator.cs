using FluentValidation;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;

internal sealed class CreateGameVersionCommandValidator : AbstractValidator<CreateGameVersionCommand>
{
    public CreateGameVersionCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();
        RuleFor(c => c.Version)
            .Must(v => VersionNumber.TryParse(v, out _))
            .WithMessage("Version must have the format MAJOR.MINOR.PATCH");
        RuleFor(c => c.ReleaseNotes).MaximumLength(GameVersion.MaxReleaseNotesLength);
    }
}
