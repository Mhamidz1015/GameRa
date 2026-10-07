using System.Security.Claims;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Infrastructure.Authentication;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Distribution.Application.Downloads.CheckForUpdate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Distribution.Presentation.Downloads;

internal sealed class CheckForUpdate : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games/{gameId:guid}/distribution/updates", async (
                Guid gameId,
                string from,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<CheckForUpdateResponse> result = await sender.Send(
                    new CheckForUpdateQuery(user.GetUserId(), gameId, from),
                    cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.DownloadGames)
            .WithTags(Tags.Distribution);
    }
}
