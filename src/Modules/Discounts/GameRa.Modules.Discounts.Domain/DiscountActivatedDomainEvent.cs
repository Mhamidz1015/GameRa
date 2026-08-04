using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Discounts.Domain
{
    public sealed class DiscountActivatedDomainEvent(Guid DiscountId) : DomainEvent
    {
        public Guid DiscountId { get; init; } = DiscountId;
    }
}