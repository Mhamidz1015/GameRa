
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Library.Infrastructure.Playtime;

internal sealed class PlaytimeRepository(LibraryItemDbContext context) : IPlaytimeRepository
{
    public async Task<PlaytimeRecord?> GetAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.PlaytimeRecords
            .FirstOrDefaultAsync(p => p.UserId == userId && p.GameId == gameId, cancellationToken);
    }

    public void Insert(PlaytimeRecord playtimeRecord)
    {
        context.PlaytimeRecords.Add(playtimeRecord);
   }
}
