using FluentValidation;
using GameRa.Modules.Distribution.Domain.GameVersions;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;

internal sealed class RegisterVersionFileCommandValidator : AbstractValidator<RegisterVersionFileCommand>
{
    public RegisterVersionFileCommandValidator(IOptions<DistributionOptions> options)
    {
        RuleFor(c => c.VersionId).NotEmpty();
        RuleFor(c => c.RelativePath).NotEmpty().MaximumLength(VersionFile.MaxPathLength);
        RuleFor(c => c.SizeBytes).GreaterThan(0).LessThanOrEqualTo(options.Value.MaxFileSizeBytes);
        RuleFor(c => c.Sha256).NotEmpty().Matches("^[0-9a-fA-F]{64}$");
    }
}
