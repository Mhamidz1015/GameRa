using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.Games;

namespace GameRa.Modules.Distribution.Application.Games.AddDistributionGame;

internal sealed class AddDistributionGameCommandHandler(
    IDistributionGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddDistributionGameCommand>
{
    public async Task<Result> Handle(AddDistributionGameCommand request, CancellationToken cancellationToken)
    {
        if (await gameRepository.ExistsAsync(request.GameId, cancellationToken))
        {
            return Result.Success();
        }

        gameRepository.Insert(DistributionGame.Create(request.GameId));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
