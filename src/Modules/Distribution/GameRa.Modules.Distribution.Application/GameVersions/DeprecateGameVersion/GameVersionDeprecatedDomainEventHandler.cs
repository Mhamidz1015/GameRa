using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.IntegrationEvents;

namespace GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;

internal sealed class GameVersionDeprecatedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<GameVersionDeprecatedDomainEvent>
{
    public override async Task Handle(
        GameVersionDeprecatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new GameVersionDeprecatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.GameId,
                domainEvent.VersionId,
                domainEvent.Version),
            cancellationToken);
    }
}
