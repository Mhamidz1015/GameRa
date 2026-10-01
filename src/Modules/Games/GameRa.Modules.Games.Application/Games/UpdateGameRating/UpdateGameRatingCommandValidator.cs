using FluentValidation;

namespace GameRa.Modules.Games.Application.Games.UpdateGameRating;

internal sealed class UpdateGameRatingCommandValidator : AbstractValidator<UpdateGameRatingCommand>
{
    private static readonly string[] ValidActions = ["add", "remove", "update"];

    public UpdateGameRatingCommandValidator()
    {
        RuleFor(c => c.GameId).NotEmpty();

        RuleFor(c => c.Action)
            .NotEmpty()
            .Must(a => ValidActions.Contains(a))
            .WithMessage("Action must be one of: add, remove, update.");

        When(c => c.Action == "add" || c.Action == "update", () =>
        {
            RuleFor(c => c.NewRating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");
        });

        When(c => c.Action == "remove" || c.Action == "update", () =>
        {
            RuleFor(c => c.OldRating)
                .InclusiveBetween(1, 5)
                .WithMessage("OldRating must be between 1 and 5.");
        });
    }
}