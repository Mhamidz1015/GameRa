namespace GameRa_Modules.Games.publicApi;

public interface IGamesApi
{
    Task<GameApiResponse?> GetGameApiAsync(Guid GameId, CancellationToken cancellationToken = default);
}
