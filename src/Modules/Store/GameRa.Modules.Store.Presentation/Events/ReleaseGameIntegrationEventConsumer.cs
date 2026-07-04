using GameRa.Common.Application.Exceptions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.integrationEvents;
using GameRa.Modules.Store.Application.Games.AddGame;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Events;

public sealed class ReleaseGameIntegrationEventConsumer(ISender sender)
    : IConsumer<ReleaseGameIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ReleaseGameIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new AddGameCommand(
                context.Message.GameId,
                context.Message.Title,
                context.Message.Description,
                context.Message.Developer,
                context.Message.Baseprice,
                context.Message.ReleaseDate,
                context.Message.Coverimgageurl),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(AddGameCommand), result.Error);
        }
    }
}
