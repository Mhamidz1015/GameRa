using System.Data.Common;
using Dapper;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Application.Abstractions.Messaging;
using GameRa.Modules.Games.Application.Games.GetGames;
using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.SearchGames;

internal sealed class SearchGamesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchGamesQuery, SearchGamesResponse>
{
    public async Task<Result<SearchGamesResponse>> Handle(SearchGamesQuery request, CancellationToken cancellationToken)
    {
        using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new SearchGamesParameters(
            (int)GameStatus.Released,
            request.CategoryId,
            request.SearchTerm is null ? null : $"%{request.SearchTerm}%",
            request.PageSize,
            (request.Page - 1) * request.PageSize);

        const string sql = """
            SELECT
                g.id AS Id,
                g.title AS Title,
                g.description AS Description,
                g.developer AS Developer,
                g.release_date AS ReleaseDate,
                g.base_price AS BasePrice,
                g.cover_image_url AS CoverImageUrl
            FROM games.games g
            WHERE
                g.status = @Status AND
                (@GenreId IS NULL OR EXISTS (
                    SELECT 1 FROM games.game_genres gg
                    WHERE gg.game_id = g.id AND gg.genre_id = @GenreId)) AND
                (@SearchTerm IS NULL OR g.title ILIKE @SearchTerm)
            ORDER BY g.release_date DESC
            OFFSET @Skip
            LIMIT @Take
            """;

        List<GameResponse> games = (await connection.QueryAsync<GameResponse>(sql, parameters)).AsList();

        int totalCount = await CountGamesAsync(connection, parameters);

        return new SearchGamesResponse(request.Page, request.PageSize, totalCount, games);
    }

    private static async Task<int> CountGamesAsync(DbConnection connection, SearchGamesParameters parameters)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM games.games g
            WHERE
                g.status = @Status AND
                (@GenreId IS NULL OR EXISTS (
                    SELECT 1 FROM games.game_genres gg
                    WHERE gg.game_id = g.id AND gg.genre_id = @GenreId)) AND
                (@SearchTerm IS NULL OR g.title ILIKE @SearchTerm)
            """;

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    private sealed record SearchGamesParameters(
        int Status,
        Guid? GenreId,
        string? SearchTerm,
        int Take,
        int Skip);
}
