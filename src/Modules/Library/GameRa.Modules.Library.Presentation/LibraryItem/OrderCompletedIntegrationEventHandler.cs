using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;
using GameRa.Modules.Store.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class OrderCompletedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<OrderCompletedIntegrationEvent>
{
    public override async Task Handle(
        OrderCompletedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        foreach (OrderCompletedGameModel game in integrationEvent.Games)
        {
            Result result = await sender.Send(
                new AddGameToLibraryCommand(
                    integrationEvent.CustomerId,
                    game.GameId,
                    game.GameTitle),
                cancellationToken);

            if (result.IsFailure)
            {
                throw new GameRaException(
                    nameof(AddGameToLibraryCommand),
                    result.Error);
            }
        }
    }
}