using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Discounts.IntegrationEvents;

public sealed class DiscountActivatedIntegrationEvent : IntegrationEvent
{
    public DiscountActivatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid discountId,
        int scope,
        Guid? gameId,
        Guid? categoryId,
        int type,
        decimal amount)
        : base(id, occurredOnUtc)
    {
        DiscountId = discountId;
        Scope = scope;
        GameId = gameId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
    }

    public Guid DiscountId { get; init; }

    // 1=Global, 2=Category, 3=Game
    public int Scope { get; init; }

    public Guid? GameId { get; init; }

    public Guid? CategoryId { get; init; }

    // 1=Percentage, 2=Fixed
    public int Type { get; init; }

    public decimal Amount { get; init; }
}
