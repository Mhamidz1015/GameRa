using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Library.Domain.LibraryItems;

public static class LibraryItemErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "LibraryItem.NotFound",
        "Library item was not found.");

    public static readonly Error AlreadyArchived = Error.Problem(
        "LibraryItem.AlreadyArchived",
        "This library item is already archived.");

    public static Error NotOwned(Guid gameId) => Error.NotFound(
        "LibraryItem.NotOwned",
        $"User does not own game with id '{gameId}'.");
}