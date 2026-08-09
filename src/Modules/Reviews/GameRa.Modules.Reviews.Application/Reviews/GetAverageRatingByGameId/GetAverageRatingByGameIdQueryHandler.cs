using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;

internal sealed class GetAverageRatingByGameIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetAverageRatingByGameIdQuery, AverageRatingResponse>
{
    public async Task<Result<AverageRatingResponse>> Handle(
        GetAverageRatingByGameIdQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 @GameId AS {nameof(AverageRatingResponse.GameId)},
                 COALESCE(AVG(rating::NUMERIC), 0) AS {nameof(AverageRatingResponse.AverageRating)},
                 COUNT(*) AS {nameof(AverageRatingResponse.TotalReviews)}
             FROM reviews.reviews
             WHERE game_id = @GameId
             """;

        AverageRatingResponse response = await connection.QuerySingleAsync<AverageRatingResponse>(
            sql,
            new { request.GameId });

        return response;
    }
}