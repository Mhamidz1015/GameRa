using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Library.Domain.LibraryItems;

public sealed class LibraryItemCreatedDomainEvent(Guid libraryId, Guid userId, Guid gameId) : DomainEvent
{
    public Guid LibraryId { get; init; } = libraryId;

    public Guid UserId { get; init; } = userId;

    public Guid GameId { get; init; } = gameId;
}
