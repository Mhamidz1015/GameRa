using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Games.GetGames;

public sealed record GetGamesQuery : IQuery<IReadOnlyCollection<GameResponse>>;
