using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Reviews.Application.Reviews.CreateReview;

internal sealed class ReviewCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<ReviewCreatedDomainEvent>
{
    public override async Task Handle(
        ReviewCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<ReviewResponse> result = await sender.Send(
            new GetReviewQuery(domainEvent.ReviewId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetReviewQuery), result.Error);
        }

        ReviewResponse review = result.Value;

        await eventBus.PublishAsync(
            new ReviewCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                review.ReviewId,
                review.GameId,
                review.UserId,
                review.Rating,
                review.IsVerifiedPurchase),
            cancellationToken);
    }
}