using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.UnarchiveLibraryItem;

public sealed record UnarchiveLibraryItemCommand(Guid UserId, Guid GameId) : ICommand;