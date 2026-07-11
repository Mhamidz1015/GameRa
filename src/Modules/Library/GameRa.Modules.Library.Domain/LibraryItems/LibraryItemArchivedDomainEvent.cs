using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Library.Domain.LibraryItems;

public sealed class LibraryItemArchivedDomainEvent(Guid libraryId) : DomainEvent
{
    public Guid LibraryId { get; init; } = libraryId;
}
