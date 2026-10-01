using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.IntegrationEvents;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class ReviewDeletedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<ReviewDeletedDomainEvent>
{
    public override async Task Handle(ReviewDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new ReviewDeletedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            domainEvent.ReviewId,
            domainEvent.GameId,
            domainEvent.Rating),
        cancellationToken);
    }
}