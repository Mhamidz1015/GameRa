using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Ownerships.ApplyGameOwnership;
using GameRa.Modules.Library.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Distribution.Presentation.IntegrationEvent;

internal sealed class LibraryItemRemovedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<LibraryItemRemovedIntegrationEvent>
{
    public override async Task Handle(
        LibraryItemRemovedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new ApplyGameOwnershipCommand(
                integrationEvent.UserId,
                integrationEvent.GameId,
                false,
                integrationEvent.OccurredOnUtc),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(ApplyGameOwnershipCommand), result.Error);
        }
    }
}
