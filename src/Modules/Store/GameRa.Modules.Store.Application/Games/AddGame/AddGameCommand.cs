using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Games.AddGame;

public sealed record AddGameCommand(
    Guid Id,
        string Title,
        string Description,
        string Developer,
        decimal BasePrice,
        DateTime ReleasedDate,
        string CoverImageUrl) : ICommand;

