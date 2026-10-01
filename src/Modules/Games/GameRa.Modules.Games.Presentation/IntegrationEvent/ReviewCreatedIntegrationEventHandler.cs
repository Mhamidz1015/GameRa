using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.UpdateGameRating;
using GameRa.Modules.Reviews.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Games.Presentation.Games;

internal sealed class ReviewCreatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<ReviewCreatedIntegrationEvent>
{
    public override async Task Handle(
        ReviewCreatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateGameRatingCommand(integrationEvent.GameId, "add", integrationEvent.Rating),
            cancellationToken);

        if (result.IsFailure)
            throw new GameRaException(nameof(UpdateGameRatingCommand), result.Error);
    }
}