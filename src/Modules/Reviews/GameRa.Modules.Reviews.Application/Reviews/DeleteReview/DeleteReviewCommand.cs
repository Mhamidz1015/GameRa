using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.DeleteReview;

public sealed record DeleteReviewCommand(Guid ReviewId, Guid UserId) : ICommand;