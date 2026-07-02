namespace GameRa_Modules.Games.publicApi;

public sealed record GameApiResponse(
    Guid GameId,
    string Name,
    decimal Price,
    string Currencyy);
