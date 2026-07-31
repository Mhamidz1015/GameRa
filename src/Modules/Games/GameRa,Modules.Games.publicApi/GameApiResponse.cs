namespace GameRa_Modules.Games.PublicApi;

public sealed record GameApiResponse(
    Guid GameId,
    string Name,
    decimal Price);
