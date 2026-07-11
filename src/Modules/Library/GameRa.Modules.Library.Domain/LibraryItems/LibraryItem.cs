using GameRa.Common.Application.Clock;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Library.Domain.LibraryItems;

public sealed class LibraryItem : Entity
{
    private LibraryItem()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid GameId { get; private set; }

    public string GametitleSnapshot { get; private set; }

    public bool IsArchived { get; private set; }


    public static LibraryItem Create(
        Guid userid,
        Guid gameid,
        string gametitlesnapshot)
    {
        var libraryitem = new LibraryItem
        {
            Id = Guid.NewGuid(),
            UserId = userid,
            GameId = gameid,
            GametitleSnapshot = gametitlesnapshot,
            IsArchived = false
        };

        libraryitem.Raise(new LibraryItemCreatedDomainEvent(libraryitem.Id));

        return libraryitem;
    }

    public Result Archive ()
    {
        if (IsArchived)
            return Result.Failure(LibraryItemErrors.AlreadyArchived);

        IsArchived = true;
        Raise(new LibraryItemArchivedDomainEvent(Id));
       
        return Result.Success();
    }
}
