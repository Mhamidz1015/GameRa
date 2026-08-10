namespace GameRa_Modules.Games.publicApi;

public sealed record GameApiResponse(
    Guid GameId,
    Guid CategoryId,
    string Name,
    decimal Price);