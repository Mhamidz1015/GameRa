using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.IntegrationEvents;
using GameRa.Modules.Store.Application.Games.ReleaseGame;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Games;

internal sealed class ReleaseGameIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<ReleaseGameIntegrationEvent>
{
    public override async Task Handle(
        ReleaseGameIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new ReleaseGameInStoreCommand(
                integrationEvent.GameId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Developer,
                integrationEvent.Baseprice,
                integrationEvent.Coverimgageurl),
            cancellationToken);

        if (result.IsFailure)
            throw new GameRaException(nameof(ReleaseGameInStoreCommand), result.Error);
    }
}