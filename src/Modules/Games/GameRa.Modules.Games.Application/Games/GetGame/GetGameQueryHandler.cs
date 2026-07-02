using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Modules.Games.Domain.Games;
using MediatR;

namespace GameRa.Modules.Games.Application.Games.GetGame;

internal sealed class GetGameQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IRequestHandler<GetGameQuery, GameResponse?>
{
    public async Task<GameResponse?> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(GameResponse.Id)},
                 title AS {nameof(GameResponse.Title)},
                 description AS {nameof(GameResponse.Description)},
                 developer AS {nameof(GameResponse.Developer)},
                 releaseDate AS {nameof(GameResponse.ReleaseDate)},
                 baseprice AS {nameof(GameResponse.Baseprice)},
                 CoverImageUrl AS {nameof(GameResponse.Coverimgageurl)}
             FROM Games.Games
             WHERE id = GameId
             """;

        GameResponse? game = await connection.QuerySingleOrDefaultAsync<GameResponse>(sql, request);

        return game;
    }
}
