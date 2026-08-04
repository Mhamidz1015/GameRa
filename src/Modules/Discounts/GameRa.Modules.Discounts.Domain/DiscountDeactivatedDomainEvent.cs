using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Discounts.Domain
{
    public sealed class DiscountDeactivatedDomainEvent(Guid DiscountId) : DomainEvent
    {
        public Guid DiscountId { get; init; } = DiscountId;
    }
}