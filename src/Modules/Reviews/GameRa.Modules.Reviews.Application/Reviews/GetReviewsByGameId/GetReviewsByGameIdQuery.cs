using GameRa.Common.Application.Messaging;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;

namespace GameRa.Modules.Reviews.Application.Reviews.GetReviewsByGameId;

public sealed record GetReviewsByGameIdQuery(Guid GameId) : IQuery<IReadOnlyCollection<ReviewResponse>>;