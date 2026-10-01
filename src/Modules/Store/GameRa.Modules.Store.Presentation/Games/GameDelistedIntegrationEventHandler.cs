using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.IntegrationEvents;
using GameRa.Modules.Store.Application.Carts.RemoveGameFromAllCarts;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Games;

internal sealed class GameDelistedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<GameDelistedIntegrationEvent>
{
    public override async Task Handle(
        GameDelistedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new RemoveGameFromAllCartsCommand(integrationEvent.GameId),
            cancellationToken);

        if (result.IsFailure)
            throw new GameRaException(nameof(RemoveGameFromAllCartsCommand), result.Error);
    }
}