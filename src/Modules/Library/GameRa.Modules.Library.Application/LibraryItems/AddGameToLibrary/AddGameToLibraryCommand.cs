using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;

public sealed record AddGameToLibraryCommand(
    Guid UserId,
    Guid GameId,
    string GameTitleSnapshot)
    : ICommand;
