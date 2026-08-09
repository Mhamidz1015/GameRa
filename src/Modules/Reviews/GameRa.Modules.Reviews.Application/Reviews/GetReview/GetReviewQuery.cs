using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.GetReview;

public sealed record GetReviewQuery(Guid ReviewId) : IQuery<ReviewResponse>;