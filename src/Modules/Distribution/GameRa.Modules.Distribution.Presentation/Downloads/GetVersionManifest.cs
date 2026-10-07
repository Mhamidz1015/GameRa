using System.Security.Claims;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.Downloads.GetVersionManifest;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.Downloads;

internal sealed class GetVersionManifest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games/{gameId:guid}/distribution/versions/{versionId:guid}/manifest", async (
                Guid gameId,
                Guid versionId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<VersionManifestResponse> result = await sender.Send(
                    new GetVersionManifestQuery(user.GetUserId(), gameId, versionId),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.DownloadGames)
            .WithTags(Tags.Distribution);
    }
}
