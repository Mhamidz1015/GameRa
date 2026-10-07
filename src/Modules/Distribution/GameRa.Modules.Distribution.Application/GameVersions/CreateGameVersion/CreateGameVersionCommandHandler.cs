using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;

internal sealed class CreateGameVersionCommandHandler(
    IDistributionGameRepository gameRepository,
    IGameVersionRepository gameVersionRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateGameVersionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGameVersionCommand request, CancellationToken cancellationToken)
    {
        if (!await gameRepository.ExistsAsync(request.GameId, cancellationToken))
        {
            return Result.Failure<Guid>(DistributionErrors.GameNotFound(request.GameId));
        }

        if (!VersionNumber.TryParse(request.Version, out VersionNumber number))
        {
            return Result.Failure<Guid>(GameVersionErrors.InvalidVersionNumber(request.Version));
        }

        if (await gameVersionRepository.ExistsAsync(request.GameId, number, cancellationToken))
        {
            return Result.Failure<Guid>(GameVersionErrors.VersionAlreadyExists(request.GameId, number.ToString()));
        }

        Result<GameVersion> result = GameVersion.CreateDraft(
            request.GameId,
            number,
            request.ReleaseNotes,
            dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        gameVersionRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;
    }
}
