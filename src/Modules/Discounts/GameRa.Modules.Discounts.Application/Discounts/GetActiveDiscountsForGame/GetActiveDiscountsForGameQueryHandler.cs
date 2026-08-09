using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;

internal sealed class GetActiveDiscountsForGameQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetActiveDiscountsForGameQuery, IReadOnlyCollection<DiscountResponse>>
{
    public async Task<Result<IReadOnlyCollection<DiscountResponse>>> Handle(
        GetActiveDiscountsForGameQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 discount_id AS {nameof(DiscountResponse.DiscountId)},
                 code AS {nameof(DiscountResponse.Code)},
                 type AS {nameof(DiscountResponse.Type)},
                 amount AS {nameof(DiscountResponse.Amount)},
                 scope AS {nameof(DiscountResponse.Scope)},
                 game_id AS {nameof(DiscountResponse.GameId)},
                 category_id AS {nameof(DiscountResponse.CategoryId)},
                 start_date_time_utc AS {nameof(DiscountResponse.StartDateTimeUtc)},
                 end_date_time_utc AS {nameof(DiscountResponse.EndDateTimeUtc)},
                 is_active AS {nameof(DiscountResponse.IsActive)}
             FROM discounts.discounts
             WHERE is_active = TRUE
               AND start_date_time_utc <= @UtcNow
               AND end_date_time_utc >= @UtcNow
               AND (
                   (scope = @GameScope AND game_id = @GameId)
                   OR (scope = @CategoryScope AND category_id = @CategoryId)
                   OR (scope = @GlobalScope)
               )
             """;

        var parameters = new
        {
            request.GameId,
            request.CategoryId,
            UtcNow = DateTime.UtcNow,
            GameScope = (int)DiscountScope.Game,
            CategoryScope = (int)DiscountScope.Category,
            GlobalScope = (int)DiscountScope.Global
        };

        List<DiscountResponse> discounts = (await connection.QueryAsync<DiscountResponse>(sql, parameters)).AsList();

        return discounts;
    }
}