using FluentValidation;

namespace GameRa.Modules.Library.Application.Playtime.RecordPlaytime;

internal sealed class RecordPlaytimeCommandValidator : AbstractValidator<RecordPlaytimeCommand>
{
    private const int MaxMinutesPerSession = 720;

    public RecordPlaytimeCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.GameId).NotEmpty();
        RuleFor(c => c.Minutes)
            .GreaterThan(0).WithMessage("Playtime must be greater than 0 minutes.")
            .LessThanOrEqualTo(MaxMinutesPerSession)
            .WithMessage($"A single session cannot exceed {MaxMinutesPerSession} minutes.");
    }
}