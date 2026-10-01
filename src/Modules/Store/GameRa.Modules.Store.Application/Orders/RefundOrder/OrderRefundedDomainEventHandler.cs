using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.IntegrationEvents;

namespace GameRa.Modules.Store.Application.Orders.RefundOrder;

internal sealed class OrderRefundedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<OrderRefundedDomainEvent>
{
    public override async Task Handle(
        OrderRefundedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        List<OrderCompletedGameModel> games = notification.GameIds
            .Select(gameId => new OrderCompletedGameModel
            {
                GameId = gameId,
                GameTitle = string.Empty,
                FinalPrice = 0
            })
            .ToList();

        await eventBus.PublishAsync(
            new OrderRefundedIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                notification.OrderId,
                notification.CustomerId,
                games),
            cancellationToken);
    }
}
