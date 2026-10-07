using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.RemoveVersionFile;

internal sealed class RemoveVersionFileCommandHandler(
    IGameVersionRepository gameVersionRepository,
    IFileStorage fileStorage,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveVersionFileCommand>
{
    public async Task<Result> Handle(RemoveVersionFileCommand request, CancellationToken cancellationToken)
    {
        GameVersion? gameVersion = await gameVersionRepository.GetAsync(request.VersionId, cancellationToken);

        if (gameVersion is null)
        {
            return Result.Failure(GameVersionErrors.NotFound(request.VersionId));
        }

        VersionFile? file = gameVersion.Files.SingleOrDefault(f => f.Id == request.FileId);

        Result result = gameVersion.RemoveFile(request.FileId);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await fileStorage.DeleteAsync(file!.StorageKey, cancellationToken);

        return Result.Success();
    }
}
