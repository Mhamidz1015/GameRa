using FluentValidation;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;

internal sealed class CheckForUpdateQueryValidator : AbstractValidator<CheckForUpdateQuery>
{
    public CheckForUpdateQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.GameId).NotEmpty();
        RuleFor(q => q.From)
            .Must(v => VersionNumber.TryParse(v, out _))
            .WithMessage("From must have the format MAJOR.MINOR.PATCH");
    }
}
