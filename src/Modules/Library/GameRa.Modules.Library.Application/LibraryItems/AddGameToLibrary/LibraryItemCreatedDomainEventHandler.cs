using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationEvents;

namespace GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;

internal sealed class LibraryItemCreatedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<LibraryItemCreatedDomainEvent>
{
    public override async Task Handle(
        LibraryItemCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new LibraryItemAddedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.UserId,
                domainEvent.GameId),
            cancellationToken);
    }
}
