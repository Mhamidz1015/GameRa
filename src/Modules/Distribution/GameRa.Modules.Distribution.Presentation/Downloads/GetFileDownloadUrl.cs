using System.Security.Claims;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrl;
using GameRa.Modules.Distribution.Application.Downloads;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.Downloads;

internal sealed class GetFileDownloadUrl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("distribution/files/{fileId:guid}/download", async (
                Guid fileId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<FileDownloadUrlResponse> result = await sender.Send(
                    new GetFileDownloadUrlQuery(user.GetUserId(), fileId),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.DownloadGames)
            .WithTags(Tags.Distribution);
    }
}
