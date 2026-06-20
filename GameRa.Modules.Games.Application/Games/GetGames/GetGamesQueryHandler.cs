using System.Data.Common;
using Dapper;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Application.Abstractions.Messaging;
using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Application.Games.GetGames;

internal sealed class GetGamesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetGamesQuery, IReadOnlyCollection<GameResponse>>
{
    public async Task<Result<IReadOnlyCollection<GameResponse>>> Handle(
        GetGamesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 SELECT
                 id AS {nameof(GameResponse.Id)},
                 title AS {nameof(GameResponse.Title)},
                 description AS {nameof(GameResponse.Description)},
                 developer AS {nameof(GameResponse.Developer)},
                 releaseDate AS {nameof(GameResponse.ReleaseDate)},
                 baseprice AS {nameof(GameResponse.Baseprice)},
                 CoverImageUrl AS {nameof(GameResponse.Coverimgageurl)}
             FROM Games.Games
             """;

        List<GameResponse> Games = (await connection.QueryAsync<GameResponse>(sql, request)).AsList();

        return Games;
    }
}
