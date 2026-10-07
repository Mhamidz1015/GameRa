using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;

internal sealed class MarkVersionReadyCommandHandler(
    IGameVersionRepository gameVersionRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<MarkVersionReadyCommand>
{
    public async Task<Result> Handle(MarkVersionReadyCommand request, CancellationToken cancellationToken)
    {
        GameVersion? gameVersion = await gameVersionRepository.GetAsync(request.VersionId, cancellationToken);

        if (gameVersion is null)
        {
            return Result.Failure(GameVersionErrors.NotFound(request.VersionId));
        }

        Result result = gameVersion.MarkReady();

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
