namespace GameRa_Modules.Games.publicApi;

public interface IGamesApi
{
    Task<bool> GameExistsAsync(Guid gameId, CancellationToken cancellationToken);
    Task<GameApiResponse?> GetGameApiAsync(Guid GameId, CancellationToken cancellationToken = default);
}