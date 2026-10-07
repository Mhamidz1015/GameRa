using System.Security.Claims;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;
using GameRa.Modules.Distribution.Application.Downloads;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.Downloads;

internal sealed class GetFileDownloadUrls : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("distribution/files/download-urls", async (
                Request request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<IReadOnlyCollection<FileDownloadUrlResponse>> result = await sender.Send(
                    new GetFileDownloadUrlsQuery(user.GetUserId(), request.FileIds),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.DownloadGames)
            .WithTags(Tags.Distribution);
    }

    internal sealed class Request
    {
        public IReadOnlyCollection<Guid> FileIds { get; init; } = [];
    }
}
