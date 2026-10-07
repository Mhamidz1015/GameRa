using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;
using MediatR;

namespace GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrl;

internal sealed class GetFileDownloadUrlQueryHandler(ISender sender)
    : IQueryHandler<GetFileDownloadUrlQuery, FileDownloadUrlResponse>
{
    public async Task<Result<FileDownloadUrlResponse>> Handle(
        GetFileDownloadUrlQuery request,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyCollection<FileDownloadUrlResponse>> result = await sender.Send(
            new GetFileDownloadUrlsQuery(request.UserId, [request.FileId]),
            cancellationToken);

        return result.IsSuccess
            ? result.Value.Single()
            : Result.Failure<FileDownloadUrlResponse>(result.Error);
    }
}
