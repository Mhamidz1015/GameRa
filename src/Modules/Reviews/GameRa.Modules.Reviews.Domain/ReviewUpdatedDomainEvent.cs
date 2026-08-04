using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Domain;

public sealed class ReviewUpdatedDomainEvent(Guid ReviewId) : DomainEvent
{
    public Guid ReviewId { get; init; } = ReviewId;
}