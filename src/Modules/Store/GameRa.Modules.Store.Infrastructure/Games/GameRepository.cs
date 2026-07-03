using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Store.Infrastructure.Games;

internal sealed class GameRepository(StoreDbContext context) : IGameRepository
{
    public async Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public void Insert(Game @event)
    {
        context.Games.Add(@event);
    }
}
