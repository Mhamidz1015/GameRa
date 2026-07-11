using GameRa.Common.Application.Data;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;
using MediatR;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class AddGameCommandHandler(IGameRepository GameRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddGameCommand, Guid>
{
    public async Task<Guid> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        var game = Game.Create(
            request.Title,
            request.Description,
            request.Developer,
            request.ReleaseDate,
            request.BasePrice,
            request.CoverImageUrl);

        GameRepository.Insert(game);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
