using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Application.Abstractions.Messaging;
using GameRa.Modules.Games.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

internal sealed class ReleaseGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ReleaseGameCommand>
{
    public async Task<Result> Handle(ReleaseGameCommand request, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.NotFound(request.GameId));
        }

        Result result = game.Release();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
