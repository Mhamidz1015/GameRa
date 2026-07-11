using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.DelistGame;

internal sealed class DelistGameCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DelistGameCommand>
{
    public async Task<Result> Handle(DelistGameCommand request, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.NotFound(request.GameId));
        }

        Result result = game.Delist(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
