namespace GameRa.Modules.Games.Application.Games.GetGame;

public sealed record GameResponse(
    Guid Id,
    Guid CategoryId,
    string Title,
    string Description,
    string Developer,
    DateTime ReleaseDate,
    decimal Baseprice,
    string Coverimgageurl);
