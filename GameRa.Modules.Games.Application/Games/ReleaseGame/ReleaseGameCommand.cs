using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

public sealed record ReleaseGameCommand(Guid GameId) : ICommand;
