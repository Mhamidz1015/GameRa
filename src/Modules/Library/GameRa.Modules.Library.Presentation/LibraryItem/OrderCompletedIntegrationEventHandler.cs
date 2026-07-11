using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;
using GameRa.Modules.Store.integrationEvents;
using MediatR;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class OrderCompletedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<OrderCompletedIntegrationEvent>
{
    public override async Task Handle(OrderCompletedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new AddGameToLibraryCommand(
                integrationEvent.CustomerId,
                integrationEvent.GameId,
                integrationEvent.GameTitle),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(
                nameof(AddGameToLibraryCommand),
                result.Error);
        }
    }
}