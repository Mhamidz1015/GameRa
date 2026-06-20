using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Games.DelistGame;

public sealed record DelistGameCommand(Guid GameId) : ICommand;
