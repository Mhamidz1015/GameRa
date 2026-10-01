using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.UpdateGameRating;

internal sealed class UpdateGameRatingCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateGameRatingCommand>
{
    public async Task<Result> Handle(UpdateGameRatingCommand request, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetAsync(request.GameId, cancellationToken);

        if (game is null)
            return Result.Failure(GameErrors.NotFound(request.GameId));

        switch (request.Action)
        {
            case "add":
                game.UpdateRating(request.NewRating);
                break;
            case "remove":
                game.RemoveRating(request.OldRating);
                break;
            case "update":
                game.RemoveRating(request.OldRating);
                game.UpdateRating(request.NewRating);
                break;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}