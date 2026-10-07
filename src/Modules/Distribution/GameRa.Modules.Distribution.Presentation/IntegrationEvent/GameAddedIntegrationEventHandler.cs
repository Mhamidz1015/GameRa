using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Games.AddDistributionGame;
using GameRa.Modules.Games.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Distribution.Presentation.IntegrationEvent;

internal sealed class GameAddedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<GameAddedIntegrationEvent>
{
    public override async Task Handle(
        GameAddedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new AddDistributionGameCommand(integrationEvent.GameId), cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(AddDistributionGameCommand), result.Error);
        }
    }
}
