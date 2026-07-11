using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;

public sealed record ArchivedLibraryItemCommand(Guid UserId, Guid GameId) : ICommand;
