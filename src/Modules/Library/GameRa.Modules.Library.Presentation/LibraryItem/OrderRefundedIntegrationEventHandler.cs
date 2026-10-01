using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.RemoveGameFromLibrary;
using GameRa.Modules.Store.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class OrderRefundedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<OrderRefundedIntegrationEvent>
{
    public override async Task Handle(
        OrderRefundedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        foreach (OrderCompletedGameModel game in integrationEvent.Games)
        {
            Result result = await sender.Send(
                new RemoveGameFromLibraryCommand(integrationEvent.CustomerId, game.GameId),
                cancellationToken);

            if (result.IsFailure)
            {
                throw new GameRaException(nameof(RemoveGameFromLibraryCommand), result.Error);
            }
        }
    }
}
