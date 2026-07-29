using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;

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
