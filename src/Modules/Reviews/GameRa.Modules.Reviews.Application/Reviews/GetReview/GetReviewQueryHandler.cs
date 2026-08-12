using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.GetReview;

internal sealed class GetReviewQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetReviewQuery, ReviewResponse>
{
    public async Task<Result<ReviewResponse>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
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
                 updated_at_utc AS {nameof(ReviewResponse.UpdatedAtUtc)}
             FROM reviews.reviews
             WHERE id = @ReviewId
             """;

        ReviewResponse? review = await connection.QuerySingleOrDefaultAsync<ReviewResponse>(sql, request);

        if (review is null)
        {
            return Result.Failure<ReviewResponse>(ReviewErrors.NotFound(request.ReviewId));
        }

        return review;
    }
}