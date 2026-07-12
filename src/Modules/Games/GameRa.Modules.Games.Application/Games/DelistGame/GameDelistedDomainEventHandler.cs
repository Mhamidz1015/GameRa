using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.IntegrationEvents;

namespace GameRa.Modules.Games.Application.Games.DelistGame;

internal sealed class GameDelistedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<GameDelistedDomainEvent>
{
    public override async Task Handle(
        GameDelistedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new GameDelistedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.GameId),
            cancellationToken);
    }
}
