using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Games;
using MediatR;
using System.Data.Common;

namespace GameRa.Modules.Games.Application.Games.GetGame;

internal sealed class GetGameQueryHandler(IDbConnectionFactory dbConnectionFactory)
   : IQueryHandler<GetGameQuery, GameResponse?>
{
    public async Task<Result<GameResponse?>> Handle(
        GetGameQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(GameResponse.Id)},
                 category_id AS {nameof(GameResponse.CategoryId)},
                 title AS {nameof(GameResponse.Title)},
                 description AS {nameof(GameResponse.Description)},
                 developer AS {nameof(GameResponse.Developer)},
                 release_date AS {nameof(GameResponse.ReleaseDate)},
                 base_price AS {nameof(GameResponse.Baseprice)},
                 cover_image_url AS {nameof(GameResponse.Coverimgageurl)}
             FROM Games.Games
             WHERE id = @GameId
             """;

        GameResponse? game =await connection.QuerySingleOrDefaultAsync<GameResponse>(sql, request);

        if (game is null)
        {
            return Result.Failure<GameResponse?>(
                GameErrors.NotFound(request.GameId));
        }

        return Result.Success<GameResponse?>(game);
    }
}