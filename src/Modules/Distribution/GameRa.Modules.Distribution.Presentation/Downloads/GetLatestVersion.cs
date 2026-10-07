using System.Security.Claims;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.Downloads.GetLatestVersion;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.Downloads;

internal sealed class GetLatestVersion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games/{gameId:guid}/distribution/latest", async (
                Guid gameId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<LatestVersionResponse> result = await sender.Send(
                    new GetLatestVersionQuery(user.GetUserId(), gameId),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.DownloadGames)
            .WithTags(Tags.Distribution);
    }
}
