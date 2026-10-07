using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;

public sealed record GetFileDownloadUrlsQuery(Guid UserId, IReadOnlyCollection<Guid> FileIds)
    : IQuery<IReadOnlyCollection<FileDownloadUrlResponse>>;
