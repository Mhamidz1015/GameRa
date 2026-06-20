using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Games.SearchGames;

public sealed record SearchGamesQuery(
    Guid? CategoryId,
    string? SearchTerm,
    int Page,
    int PageSize) : IQuery<SearchGamesResponse>;
