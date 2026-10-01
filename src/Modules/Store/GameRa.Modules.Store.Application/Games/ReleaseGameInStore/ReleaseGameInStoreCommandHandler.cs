using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Abstractions.Data;
using GameRa.Modules.Store.Domain.Games;

namespace GameRa.Modules.Store.Application.Games.ReleaseGameInStore;

internal sealed class ReleaseGameInStoreCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ReleaseGameInStoreCommand>
{
    public async Task<Result> Handle(
        ReleaseGameInStoreCommand request,
        CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetAsync(request.GameId, cancellationToken);

        if (game is null)
            return Result.Failure(GameErrors.NotFound(request.GameId));

        game.Release(
            request.Title,
            request.Description,
            request.Developer,
            request.BasePrice,
            request.CoverImageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}