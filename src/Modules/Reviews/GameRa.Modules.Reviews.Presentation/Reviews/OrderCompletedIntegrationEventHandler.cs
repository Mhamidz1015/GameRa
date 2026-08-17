using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.AddVerifiedPurchase;
using GameRa.Modules.Store.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Reviews.Presentation.Reviews;

internal sealed class OrderCompletedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<OrderCompletedIntegrationEvent>
{
    public override async Task Handle(
        OrderCompletedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        foreach (OrderCompletedGameModel game in integrationEvent.Games)
        {
            Result result = await sender.Send(
                new AddVerifiedPurchaseCommand(
                    game.GameId,
                    integrationEvent.CustomerId,
                    integrationEvent.OccurredOnUtc),
                cancellationToken);

            if (result.IsFailure)
            {
                throw new GameRaException(
                    nameof(AddVerifiedPurchaseCommand),
                    result.Error);
            }
        }
    }
}