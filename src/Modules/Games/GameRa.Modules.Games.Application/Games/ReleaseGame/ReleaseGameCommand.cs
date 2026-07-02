using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

public sealed record ReleaseGameCommand(Guid GameId) : ICommand;
