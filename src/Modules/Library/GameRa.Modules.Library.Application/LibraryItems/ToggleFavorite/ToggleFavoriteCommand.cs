using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Library.Application.LibraryItems.ToggleFavorite;

public sealed record ToggleFavoriteCommand(Guid UserId, Guid GameId) : ICommand;