using GameRa.Common.Application.Messaging;
using GameRa.Modules.Reviews.Domain;

namespace GameRa.Modules.Reviews.Application.Reviews.UpdateReview;

// ReviewUpdated does not need an Integration Event
// Games module only needs to know about Created/Deleted to recalculate AverageRating
// Update changes Rating in place — Games will recalculate on next ReviewCreated/Deleted
// No Reason :(
internal sealed class ReviewUpdatedDomainEventHandler
    : DomainEventHandler<ReviewUpdatedDomainEvent>
{
    public override Task Handle(
        ReviewUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}