using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;

public sealed record ApplyGameOwnershipCommand(Guid UserId, Guid GameId, bool Owned, DateTime OccurredOnUtc) : ICommand;
