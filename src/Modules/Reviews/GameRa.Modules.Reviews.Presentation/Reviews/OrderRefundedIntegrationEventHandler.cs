using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.RemoveVerifiedPurchase;
using GameRa.Modules.Store.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

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
                new RemoveVerifiedPurchaseCommand(game.GameId, integrationEvent.CustomerId),
                cancellationToken);

            if (result.IsFailure)
            {
                throw new GameRaException(nameof(RemoveVerifiedPurchaseCommand), result.Error);
            }
        }
    }
}
