namespace GameRa.Modules.Library.Domain.LibraryItems;

public interface ILibraryItemRepository
{
    Task<LibraryItem?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid UserId, Guid GameId, CancellationToken cancellationToken = default);

    Task<LibraryItem?> GetByUserAndGameAsync(
        Guid userId,
        Guid gameId,
        CancellationToken cancellationToken = default);

    void Insert(LibraryItem libraryItem);
}
