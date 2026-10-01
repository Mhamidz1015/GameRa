using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class ReviewUpdatedDomainEventHandler(
    ISender sender,
    IEventBus eventBus)
    : DomainEventHandler<ReviewUpdatedDomainEvent>
{
    public override async Task Handle(ReviewUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Result<ReviewResponse> result = await sender.Send(
            new GetReviewQuery(domainEvent.ReviewId), cancellationToken);

        if (result.IsFailure)
            throw new GameRaException(nameof(GetReviewQuery), result.Error);

        ReviewResponse review = result.Value;

        await eventBus.PublishAsync(new ReviewUpdatedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            review.ReviewId,
            review.GameId,
            domainEvent.OldRating,
            review.Rating),
        cancellationToken);
    }
}