using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Library.Infrastructure.LibraryItems;

internal sealed class LibraryItemRepository(LibraryItemDbContext context) : ILibraryItemRepository
{
    public async Task<bool> ExistsAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.LibraryItems.AnyAsync(x => x.UserId == userId && x.GameId == gameId, cancellationToken);
    }

    public async Task<LibraryItem?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.LibraryItems.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<LibraryItem?> GetByUserAndGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.LibraryItems.FirstOrDefaultAsync( x => x.UserId == userId && x.GameId == gameId, cancellationToken);
    }

    public void Insert(LibraryItem libraryItem)
    {
        context.LibraryItems.Add(libraryItem);
    }
}
