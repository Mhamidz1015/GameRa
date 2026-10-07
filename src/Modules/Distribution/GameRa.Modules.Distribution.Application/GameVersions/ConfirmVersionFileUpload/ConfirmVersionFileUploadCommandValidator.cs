using FluentValidation;

namespace GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;

internal sealed class ConfirmVersionFileUploadCommandValidator : AbstractValidator<ConfirmVersionFileUploadCommand>
{
    public ConfirmVersionFileUploadCommandValidator()
    {
        RuleFor(c => c.VersionId).NotEmpty();
        RuleFor(c => c.FileId).NotEmpty();
    }
}
