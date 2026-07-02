using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Games.DelistGame;

public sealed record DelistGameCommand(Guid GameId) : ICommand;
