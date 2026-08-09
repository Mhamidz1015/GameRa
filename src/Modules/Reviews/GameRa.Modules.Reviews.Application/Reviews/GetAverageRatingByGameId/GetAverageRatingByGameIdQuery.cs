using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;

public sealed record GetAverageRatingByGameIdQuery(Guid GameId) : IQuery<AverageRatingResponse>;