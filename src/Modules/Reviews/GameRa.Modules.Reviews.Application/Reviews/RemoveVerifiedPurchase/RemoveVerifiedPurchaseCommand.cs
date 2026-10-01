using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.RemoveVerifiedPurchase;

public sealed record RemoveVerifiedPurchaseCommand(Guid GameId, Guid UserId) : ICommand;
