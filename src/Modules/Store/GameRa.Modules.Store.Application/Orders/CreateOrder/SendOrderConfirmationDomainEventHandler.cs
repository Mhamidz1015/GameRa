using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Orders.GetOrder;
using GameRa.Modules.Store.Domain.Orders;
using MediatR;

namespace GameRa.Modules.Store.Application.Orders.CreateOrder;

internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<OrderResponse> result = await sender.Send(new GetOrderQuery(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetOrderQuery), result.Error);
        }

        // Send order confirmation notification.
    }
}
