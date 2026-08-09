namespace GameRa.Modules.Games.PublicApi;

public sealed record GameApiResponse(
    Guid GameId,
    Guid CategoryId,
    string Name,
    decimal Price);