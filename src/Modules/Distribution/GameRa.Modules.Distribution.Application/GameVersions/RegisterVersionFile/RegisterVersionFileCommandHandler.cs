using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using GameRa.Modules.Distribution.Domain.GameVersions;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;

internal sealed class RegisterVersionFileCommandHandler(
    IGameVersionRepository gameVersionRepository,
    IFileStorage fileStorage,
    IDateTimeProvider dateTimeProvider,
    IOptions<DistributionOptions> options,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterVersionFileCommand, RegisterVersionFileResponse>
{
    public async Task<Result<RegisterVersionFileResponse>> Handle(
        RegisterVersionFileCommand request,
        CancellationToken cancellationToken)
    {
        GameVersion? gameVersion = await gameVersionRepository.GetAsync(request.VersionId, cancellationToken);

        if (gameVersion is null)
        {
            return Result.Failure<RegisterVersionFileResponse>(GameVersionErrors.NotFound(request.VersionId));
        }

        Result<VersionFile> result = gameVersion.AddFile(request.RelativePath, request.SizeBytes, request.Sha256);

        if (result.IsFailure)
        {
            return Result.Failure<RegisterVersionFileResponse>(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        TimeSpan ttl = options.Value.UploadUrlTtl;
        DateTime expiresAtUtc = dateTimeProvider.UtcNow.Add(ttl);

        PresignedUpload upload = await fileStorage.CreateUploadUrlAsync(
            result.Value.StorageKey,
            result.Value.SizeBytes,
            result.Value.Sha256,
            ttl,
            cancellationToken);

        return new RegisterVersionFileResponse(result.Value.Id, upload.Url, upload.Headers, expiresAtUtc);
    }
}
