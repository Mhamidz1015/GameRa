using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationEvents;

namespace GameRa.Modules.Library.Application.LibraryItems.RemoveGameFromLibrary;

internal sealed class LibraryItemRemovedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<LibraryItemRemovedDomainEvent>
{
    public override async Task Handle(
        LibraryItemRemovedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new LibraryItemRemovedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.UserId,
                domainEvent.GameId),
            cancellationToken);
    }
}
