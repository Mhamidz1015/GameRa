using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.UpdateGameRating;
using GameRa.Modules.Reviews.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Games.Presentation.IntegrationEvent;

internal sealed class ReviewUpdatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<ReviewUpdatedIntegrationEvent>
{
    public override async Task Handle(
        ReviewUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateGameRatingCommand(
                integrationEvent.GameId, "update",
                integrationEvent.NewRating, integrationEvent.OldRating),
            cancellationToken);

        if (result.IsFailure)
            throw new GameRaException(nameof(UpdateGameRatingCommand), result.Error);
    }
}