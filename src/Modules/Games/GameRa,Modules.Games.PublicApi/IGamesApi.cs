namespace GameRa_Modules.Games.PublicApi;

public interface IGamesApi
{
    Task<GameApiResponse?> GetGameApiAsync(Guid GameId, CancellationToken cancellationToken = default);
}
