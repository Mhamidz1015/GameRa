using System.Data.Common;
using FluentValidation;
using GameRa.Common.Application.Clock;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using GameRa.Modules.Distribution.Application.Abstractions.Validation;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;

internal sealed class GetFileDownloadUrlsQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IGameOwnershipReader ownershipReader,
    IFileStorage fileStorage,
    IDateTimeProvider dateTimeProvider,
    IOptions<DistributionOptions> options,
    IValidator<GetFileDownloadUrlsQuery> validator)
    : IQueryHandler<GetFileDownloadUrlsQuery, IReadOnlyCollection<FileDownloadUrlResponse>>
{
    public async Task<Result<IReadOnlyCollection<FileDownloadUrlResponse>>> Handle(
        GetFileDownloadUrlsQuery request,
        CancellationToken cancellationToken)
    {
        if (await validator.ValidateRequestAsync(request, cancellationToken) is { } validationError)
        {
            return Result.Failure<IReadOnlyCollection<FileDownloadUrlResponse>>(validationError);
        }

        Guid[] fileIds = request.FileIds.Distinct().ToArray();

        IReadOnlyCollection<DownloadableFile> files;
        await using (DbConnection connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            files = await DownloadableFiles.GetAsync(connection, fileIds);
        }

        Guid? missingFileId = fileIds.Cast<Guid?>().FirstOrDefault(id => files.All(f => f.FileId != id));

        if (missingFileId is not null)
        {
            return Result.Failure<IReadOnlyCollection<FileDownloadUrlResponse>>(
                GameVersionErrors.FileNotFound(missingFileId.Value));
        }

        DownloadableFile? hidden = files.FirstOrDefault(f => !f.IsDownloadable);

        if (hidden is not null)
        {
            return Result.Failure<IReadOnlyCollection<FileDownloadUrlResponse>>(
                GameVersionErrors.NotDownloadable(hidden.FileId));
        }

        foreach (Guid gameId in files.Select(f => f.GameId).Distinct())
        {
            if (!await ownershipReader.OwnsAsync(request.UserId, gameId, cancellationToken))
            {
                return Result.Failure<IReadOnlyCollection<FileDownloadUrlResponse>>(
                    DistributionErrors.GameNotOwned(gameId));
            }
        }

        TimeSpan ttl = options.Value.DownloadUrlTtl;
        DateTime expiresAtUtc = dateTimeProvider.UtcNow.Add(ttl);

        var responses = new List<FileDownloadUrlResponse>(files.Count);

        foreach (DownloadableFile file in files)
        {
            string url = await fileStorage.CreateDownloadUrlAsync(file.StorageKey, ttl, cancellationToken);

            responses.Add(new FileDownloadUrlResponse
            {
                FileId = file.FileId,
                RelativePath = file.RelativePath,
                SizeBytes = file.SizeBytes,
                Sha256 = file.Sha256,
                Url = url,
                ExpiresAtUtc = expiresAtUtc
            });
        }

        return responses;
    }
}
