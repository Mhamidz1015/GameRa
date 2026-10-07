using FluentValidation;

namespace GameRa.Modules.Distribution.Application.GameVersions.RemoveVersionFile;

internal sealed class RemoveVersionFileCommandValidator : AbstractValidator<RemoveVersionFileCommand>
{
    public RemoveVersionFileCommandValidator()
    {
        RuleFor(c => c.VersionId).NotEmpty();
        RuleFor(c => c.FileId).NotEmpty();
    }
}
