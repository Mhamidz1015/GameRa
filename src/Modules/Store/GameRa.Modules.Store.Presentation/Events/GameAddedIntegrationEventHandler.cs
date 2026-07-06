using Evently.Modules.Game.IntegrationEvents;
using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Games.AddGame;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Events;

internal sealed class GameAddedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<GameAddedIntegrationEvent>
{
    public override async Task Handle(
        GameAddedIntegrationEvent integrationEvent,
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
