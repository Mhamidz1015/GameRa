using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.GetDiscountByCode;

internal sealed class GetDiscountByCodeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetDiscountByCodeQuery, DiscountResponse>
{
    public async Task<Result<DiscountResponse>> Handle(
        GetDiscountByCodeQuery request,
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
             WHERE code = @Code
             """;

        DiscountResponse? discount = await connection.QuerySingleOrDefaultAsync<DiscountResponse>(
            sql,
            new { Code = request.Code.ToUpperInvariant() });

        if (discount is null)
        {
            return Result.Failure<DiscountResponse>(DiscountErrors.CodeNotFound(request.Code));
        }

        return discount;
    }
}