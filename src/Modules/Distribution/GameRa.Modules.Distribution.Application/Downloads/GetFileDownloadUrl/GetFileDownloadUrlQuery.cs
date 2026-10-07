using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrl;

public sealed record GetFileDownloadUrlQuery(Guid UserId, Guid FileId) : IQuery<FileDownloadUrlResponse>;
