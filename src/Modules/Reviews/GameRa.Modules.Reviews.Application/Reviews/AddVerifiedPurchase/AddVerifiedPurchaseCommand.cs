using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.AddVerifiedPurchase;

public sealed record AddVerifiedPurchaseCommand(
    Guid GameId,
    Guid UserId,
    DateTime PurchasedAtUtc) : ICommand;