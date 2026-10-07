using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;

internal sealed class ConfirmVersionFileUploadCommandHandler(
    IGameVersionRepository gameVersionRepository,
    IFileStorage fileStorage,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmVersionFileUploadCommand>
{
    public async Task<Result> Handle(ConfirmVersionFileUploadCommand request, CancellationToken cancellationToken)
    {
        GameVersion? gameVersion = await gameVersionRepository.GetAsync(request.VersionId, cancellationToken);

        if (gameVersion is null)
        {
            return Result.Failure(GameVersionErrors.NotFound(request.VersionId));
        }

        VersionFile? file = gameVersion.Files.SingleOrDefault(f => f.Id == request.FileId);

        if (file is null)
        {
            return Result.Failure(GameVersionErrors.FileNotFound(request.FileId));
        }

        if (file.UploadStatus == FileUploadStatus.Verified)
        {
            return Result.Success();
        }

        // The backend verifies the stored object itself; a wrong object is kept so it can be overwritten and confirmed again.
        StoredObjectInfo stored = await fileStorage.GetObjectInfoAsync(file.StorageKey, cancellationToken);

        if (!stored.Exists)
        {
            return Result.Failure(GameVersionErrors.UploadNotFound(file.Id));
        }

        if (stored.SizeBytes != file.SizeBytes)
        {
            return Result.Failure(GameVersionErrors.UploadSizeMismatch(file.Id));
        }

        if (!string.Equals(stored.Sha256, file.Sha256, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(GameVersionErrors.UploadChecksumMismatch(file.Id));
        }

        Result result = gameVersion.MarkFileVerified(file.Id);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
