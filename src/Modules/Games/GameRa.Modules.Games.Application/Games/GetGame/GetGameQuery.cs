using MediatR;

namespace GameRa.Modules.Games.Application.Games.GetGame;

public sealed record GetGameQuery(Guid GameId) : IRequest<GameResponse?>;
