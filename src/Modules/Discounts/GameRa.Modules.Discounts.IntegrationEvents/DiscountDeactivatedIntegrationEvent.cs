using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Discounts.IntegrationEvents;

public sealed class DiscountDeactivatedIntegrationEvent : IntegrationEvent
{
    public DiscountDeactivatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid discountId,
        int scope,
        Guid? gameId,
        Guid? categoryId)
        : base(id, occurredOnUtc)
    {
        DiscountId = discountId;
        Scope = scope;
        GameId = gameId;
        CategoryId = categoryId;
    }

    public Guid DiscountId { get; init; }

    public int Scope { get; init; }

    public Guid? GameId { get; init; }

    public Guid? CategoryId { get; init; }
}
