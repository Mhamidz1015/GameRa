using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Discounts.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Discounts.Application.Discounts.DeactivateDiscount;

internal sealed class DiscountDeactivatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<DiscountDeactivatedDomainEvent>
{
    public override async Task Handle(
        DiscountDeactivatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<DiscountResponse> result = await sender.Send(
            new GetDiscountQuery(domainEvent.DiscountId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetDiscountQuery), result.Error);
        }

        DiscountResponse discount = result.Value;

        await eventBus.PublishAsync(
            new DiscountDeactivatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                discount.DiscountId,
                discount.Scope,
                discount.GameId,
                discount.CategoryId),
            cancellationToken);
    }
}