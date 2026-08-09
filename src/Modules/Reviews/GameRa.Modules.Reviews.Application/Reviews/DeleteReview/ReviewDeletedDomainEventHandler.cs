using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.IntegrationEvents;

namespace GameRa.Modules.Reviews.Application.Reviews.DeleteReview;

internal sealed class ReviewDeletedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<ReviewDeletedDomainEvent>
{
    public override async Task Handle(
        ReviewDeletedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {

        await eventBus.PublishAsync(
            new ReviewDeletedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.ReviewId,
                domainEvent.GameId,   
                domainEvent.Rating),   
            cancellationToken);
    }
}