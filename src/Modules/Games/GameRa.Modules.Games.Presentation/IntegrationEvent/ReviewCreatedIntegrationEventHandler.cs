using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Reviews.IntegrationEvents;

namespace GameRa.Modules.Games.Presentation.IntegrationEvent;

internal sealed class ReviewCreatedIntegrationEventHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : IntegrationEventHandler<ReviewCreatedIntegrationEvent>
{
    public override async Task Handle(
        ReviewCreatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Game? game = await gameRepository.GetAsync(integrationEvent.GameId, cancellationToken);
        if (game is null) return;

        game.UpdateRating(integrationEvent.Rating);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
