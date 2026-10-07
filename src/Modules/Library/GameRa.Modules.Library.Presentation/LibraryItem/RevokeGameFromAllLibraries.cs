using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Common.Presentation.Results;
using GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Library.Presentation.LibraryItem;

internal sealed class RevokeGameFromAllLibraries : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("library/games/{gameId:guid}/revoke", async (
                Guid gameId,
                ISender sender,
                CancellationToken cancellationToken) =>
        {
            Result result = await sender.Send(
                new RevokeGameFromAllLibrariesCommand(gameId),
                cancellationToken);

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
            .RequireAuthorization(Permissions.RevokeGame)
            .WithTags(Tags.Library);
    }
}