using FluentValidation;

namespace GameRa.Modules.Reviews.Application.Reviews.DeleteReview;

internal sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(c => c.ReviewId).NotEmpty();

        RuleFor(c => c.UserId).NotEmpty();
    }
}