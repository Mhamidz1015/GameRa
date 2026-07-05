using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.integrationEvents;
using GameRa.Modules.Store.Application.Games.AddGame;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Events;

internal sealed class ReleasedGameIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<ReleaseGameIntegrationEvent>
{
    public override async Task Handle(
        ReleaseGameIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new AddGameCommand(
                integrationEvent.GameId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Developer,
                integrationEvent.Baseprice,
                integrationEvent.ReleaseDate,
                integrationEvent.Coverimgageurl),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(AddGameCommand), result.Error);
        }
    }
}
