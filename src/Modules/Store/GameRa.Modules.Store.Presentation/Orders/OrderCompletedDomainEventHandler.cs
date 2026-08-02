using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Orders.GetOrder;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Store.Presentation.Orders;

internal sealed class OrderCompletedDomainEventHandler(
    ISender sender,
    IGameRepository gameRepository,
    IEventBus eventBus)
    : DomainEventHandler<OrderCompletedDomainEvent>
{
    public override async Task Handle(
        OrderCompletedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {

        Result<OrderResponse> result = await sender.Send(
            new GetOrderQuery(notification.OrderId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetOrderQuery), result.Error);
        }

        OrderResponse order = result.Value;

        foreach (OrderItemResponse orderItem in order.OrderItems)
        {
            Game? game = await gameRepository.GetAsync(orderItem.GameId, cancellationToken);

            if (game is null)
            {
                throw new GameRaException(
                    nameof(IGameRepository),
                    GameErrors.NotFound(orderItem.GameId));
            }

            await eventBus.PublishAsync(
                new OrderCompletedIntegrationEvent(
                    notification.Id,
                    notification.OccurredOnUtc,
                    order.CustomerId,
                    game.Id,
                    game.Title),
                cancellationToken);
        }
    }
}