using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Application.Data;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Application.Abstractions.Data;

namespace GameRa.Modules.Store.Application.Games.AddGame;

internal sealed class AddGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddGameCommand>
{
    public async Task<Result> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        var game = Game.Create(
            request.Id,
            request.Title,
            request.Description,
            request.Developer,
            request.BasePrice,
            request.CoverImageUrl);

        gameRepository.Insert(game);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
