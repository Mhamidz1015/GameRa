using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Games.AddGame;

public sealed record AddGameCommand(
    Guid CategoryId,
    string Title,
    string Description,
    string Developer,
    DateTime ReleaseDate,
    decimal BasePrice,
    string CoverImageUrl) : ICommand<Guid>;
