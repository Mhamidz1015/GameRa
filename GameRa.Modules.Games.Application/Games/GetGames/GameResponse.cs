namespace GameRa.Modules.Games.Application.Games.GetGames;

public sealed record GameResponse(
    Guid Id,
    string Title,
    string Description,
    string Developer,
    DateTime ReleaseDate,
    decimal Baseprice,
    string Coverimgageurl);
