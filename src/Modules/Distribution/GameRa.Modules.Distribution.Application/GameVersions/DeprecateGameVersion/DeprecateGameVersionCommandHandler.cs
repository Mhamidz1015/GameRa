using GameRa.Common.Application.Caching;
using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Caching;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.DeprecateGameVersion;

internal sealed class DeprecateGameVersionCommandHandler(
    IGameVersionRepository gameVersionRepository,
    IDateTimeProvider dateTimeProvider,
    ICacheService cacheService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeprecateGameVersionCommand>
{
    public async Task<Result> Handle(DeprecateGameVersionCommand request, CancellationToken cancellationToken)
    {
        GameVersion? gameVersion = await gameVersionRepository.GetAsync(request.VersionId, cancellationToken);

        if (gameVersion is null)
        {
            return Result.Failure(GameVersionErrors.NotFound(request.VersionId));
        }

        Result result = gameVersion.Deprecate(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync(DistributionCacheKeys.LatestVersion(gameVersion.GameId), cancellationToken);

        return Result.Success();
    }
}
