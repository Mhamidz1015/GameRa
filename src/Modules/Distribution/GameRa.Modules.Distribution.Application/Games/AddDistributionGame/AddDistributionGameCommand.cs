using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Games.AddDistributionGame;

public sealed record AddDistributionGameCommand(Guid GameId) : ICommand;
