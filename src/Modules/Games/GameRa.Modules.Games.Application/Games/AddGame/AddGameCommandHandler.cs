using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;
using MediatR;

namespace GameRa.Modules.Games.Application.Games.AddGame;

internal sealed class AddGameCommandHandler(IGameRepository GameRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddGameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddGameCommand request, CancellationToken cancellationToken)
    {
        Result<Game> result = Game.Create(
            request.Title,
            request.Description,
            request.Developer,
            request.ReleaseDate,
            request.BasePrice,
            request.CoverImageUrl);

        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        GameRepository.Insert(result.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;
    }
}
