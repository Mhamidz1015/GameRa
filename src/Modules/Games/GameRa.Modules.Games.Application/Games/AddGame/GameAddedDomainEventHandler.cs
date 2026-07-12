using GameRa.Common.Application.Exceptions;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.GetGame;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.IntegrationEvents;
using MediatR;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class GameAddedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<GameAddedDomainEvent>
{
    public override async Task Handle(
        GameAddedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<GameResponse> result = await sender.Send(new GetGameQuery(domainEvent.GameId), cancellationToken);

        if (result.IsFailure)
        {
            throw new GameRaException(nameof(GetGameQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new GameAddedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.Title,
                result.Value.Description,
                result.Value.Developer,
                result.Value.ReleaseDate,
                result.Value.Baseprice,
                result.Value.Coverimgageurl),
            cancellationToken);
    }
}
