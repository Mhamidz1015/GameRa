using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Discounts.Domain
{
    public sealed class DiscountCreatedDomainEvent(
        Guid DiscountId,
        DiscountScope Scope,
        Guid? GameId,
        Guid? CategoryId) : DomainEvent
    {
        public Guid DiscountId { get; init; } = DiscountId;

        public DiscountScope Scope { get; init; } = Scope;

        public Guid? GameId { get; init; } = GameId;

        public Guid? CategoryId { get; init; } = CategoryId;
    }
}