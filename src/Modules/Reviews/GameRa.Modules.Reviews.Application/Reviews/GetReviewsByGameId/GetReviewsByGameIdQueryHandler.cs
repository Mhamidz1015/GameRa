using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;

namespace GameRa.Modules.Reviews.Application.Reviews.GetReviewsByGameId;

internal sealed class GetReviewsByGameIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetReviewsByGameIdQuery, IReadOnlyCollection<ReviewResponse>>
{
    public async Task<Result<IReadOnlyCollection<ReviewResponse>>> Handle(
        GetReviewsByGameIdQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 review_id AS {nameof(ReviewResponse.ReviewId)},
                 game_id AS {nameof(ReviewResponse.GameId)},
                 user_id AS {nameof(ReviewResponse.UserId)},
                 rating AS {nameof(ReviewResponse.Rating)},
                 comment AS {nameof(ReviewResponse.Comment)},
                 created_at_utc AS {nameof(ReviewResponse.CreatedAtUtc)},
                 updated_at_utc AS {nameof(ReviewResponse.UpdatedAtUtc)},
                 is_verified_purchase AS {nameof(ReviewResponse.IsVerifiedPurchase)}
             FROM reviews.reviews
             WHERE game_id = @GameId
             ORDER BY created_at_utc DESC
             """;

        List<ReviewResponse> reviews = (await connection.QueryAsync<ReviewResponse>(sql, request)).AsList();

        return reviews;
    }
}