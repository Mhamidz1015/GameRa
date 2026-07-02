using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Games.GetGames;

public sealed record GetGamesQuery : IQuery<IReadOnlyCollection<GameResponse>>;
