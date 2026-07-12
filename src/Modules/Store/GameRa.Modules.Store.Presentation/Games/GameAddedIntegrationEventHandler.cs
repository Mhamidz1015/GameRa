using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.IntegrationEvents;
using GameRa.Modules.Store.Application.Games.AddGame;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Games;

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
                integrationEvent.BasePrice,
                integrationEvent.ReleaseDate,
                integrationEvent.CoverImageUrl),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(AddGameCommand), result.Error);
        }
    }
}
