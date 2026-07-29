using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Application.Games.GetGame;

public sealed record GetGameQuery(Guid GameId) : IQuery<GameResponse?>;
