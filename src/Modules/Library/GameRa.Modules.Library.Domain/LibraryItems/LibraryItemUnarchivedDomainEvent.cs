using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Library.Domain.LibraryItems;

public sealed class LibraryItemUnarchivedDomainEvent(Guid libraryId) : DomainEvent
{
    public Guid LibraryId { get; init; } = libraryId;
}