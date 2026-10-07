using FluentValidation;

namespace GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;

internal sealed class PublishGameVersionCommandValidator : AbstractValidator<PublishGameVersionCommand>
{
    public PublishGameVersionCommandValidator()
    {
        RuleFor(c => c.VersionId).NotEmpty();
    }
}
