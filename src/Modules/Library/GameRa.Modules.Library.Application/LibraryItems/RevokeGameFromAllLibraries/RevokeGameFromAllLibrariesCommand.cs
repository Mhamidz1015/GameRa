using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;

public sealed record RevokeGameFromAllLibrariesCommand(Guid GameId) : ICommand;