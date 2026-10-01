using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Games.ReleaseGame;

public sealed record ReleaseGameInStoreCommand(
    Guid GameId,
    string Title,
    string Description,
    string Developer,
    decimal BasePrice,
    string CoverImageUrl) : ICommand;