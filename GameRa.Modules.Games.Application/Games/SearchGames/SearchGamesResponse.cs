using GameRa.Modules.Games.Application.Games.GetGames;

namespace GameRa.Modules.Games.Application.Games.SearchGames;

public sealed record SearchGamesResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<GameResponse> Games);
