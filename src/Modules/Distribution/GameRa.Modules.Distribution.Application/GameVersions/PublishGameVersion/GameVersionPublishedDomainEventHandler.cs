using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.IntegrationEvents;

namespace GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;

internal sealed class GameVersionPublishedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<GameVersionPublishedDomainEvent>
{
    public override async Task Handle(
        GameVersionPublishedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new GameVersionPublishedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.GameId,
                domainEvent.VersionId,
                domainEvent.Version),
            cancellationToken);
    }
}
