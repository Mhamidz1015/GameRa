namespace GameRa.Modules.Games.Domain.Games;

public interface IGameRepository
{
    Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Game game);
}
