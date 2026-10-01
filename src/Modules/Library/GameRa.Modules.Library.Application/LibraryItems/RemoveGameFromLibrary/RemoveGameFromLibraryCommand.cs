using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.RemoveGameFromLibrary;

public sealed record RemoveGameFromLibraryCommand(Guid UserId, Guid GameId) : ICommand;
