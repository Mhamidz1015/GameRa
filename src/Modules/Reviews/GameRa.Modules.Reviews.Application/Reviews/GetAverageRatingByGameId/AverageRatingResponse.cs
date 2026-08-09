namespace GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;

public sealed record AverageRatingResponse(Guid GameId, double AverageRating, int TotalReviews);