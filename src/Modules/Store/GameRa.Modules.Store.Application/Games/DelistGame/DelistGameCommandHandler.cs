using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Application.Data;
using GameRa.Modules.Store.Domain.Games;

namespace GameRa.Modules.Store.Application.Games.DelistGame;

internal sealed class DelistGameCommandHandler(IGameRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DelistGameCommand>
{
    public async Task<Result> Handle(DelistGameCommand request, CancellationToken cancellationToken)
    {
        Game? game = await eventRepository.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.NotFound(request.GameId));
        }

        game.Delist();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
